using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace UnitsGenerator
{
    [Generator]
    class SymbolListGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            string attribute = "UnitSymbol";

            // the generator infrastructure will create a receiver and populate it
            // we can retrieve the populated instance via the context
            UnitSyntaxReceiver syntaxReceiver = (UnitSyntaxReceiver)context.SyntaxReceiver;

            // get the recorded user class
            ClassDeclarationSyntax userClass = syntaxReceiver.ClassToAugment;

            if(userClass is null)
            {
                throw new Exception("Could not find Userclass");
            }
            var classWithAttributes = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
                    .Any(p => p.DescendantNodes().OfType<AttributeSyntax>().Any()));

            foreach (SyntaxTree tree in classWithAttributes)
            {
                SemanticModel semanticModel = context.Compilation.GetSemanticModel(tree);

                foreach (ClassDeclarationSyntax declaredClass in tree
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(cd => cd.DescendantNodes().OfType<AttributeSyntax>().Any()))
                {
                    List<SyntaxToken> nodes = declaredClass
                    .DescendantNodes()
                    .OfType<AttributeSyntax>()
                    .FirstOrDefault(a => a.DescendantTokens().Any(dt => dt.IsKind(SyntaxKind.IdentifierToken) && semanticModel.GetTypeInfo(dt.Parent).Type.Name == attribute))
                    ?.DescendantTokens()
                    ?.Where(dt => dt.IsKind(SyntaxKind.IdentifierToken))
                    ?.ToList();

                    if (nodes == null)
                    {
                        continue;
                    }

                    var relatedClass = semanticModel.GetTypeInfo(nodes.Last().Parent);

                    var generatedClass = this.GenerateClass(relatedClass);

                    foreach (MethodDeclarationSyntax classMethod in declaredClass.Members.Where(m => m.IsKind(SyntaxKind.MethodDeclaration)).OfType<MethodDeclarationSyntax>())
                    {
                        this.GenerateMethod(declaredClass.Identifier, relatedClass, classMethod, ref generatedClass);
                    }

                    this.CloseClass(generatedClass);

                    context.AddSource($"{declaredClass.Identifier}_{relatedClass.Type.Name}", SourceText.From(generatedClass.ToString(), Encoding.UTF8));
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new UnitSyntaxReceiver());
        }

        private void GenerateMethod(SyntaxToken moduleName, TypeInfo relatedClass, MethodDeclarationSyntax methodDeclaration, ref StringBuilder builder)
        {
            string signature = $"{methodDeclaration.Modifiers} {relatedClass.Type.Name} {methodDeclaration.Identifier}(";

            IEnumerable<ParameterSyntax> parameters = methodDeclaration.ParameterList.Parameters.Skip(1);

            signature += string.Join(", ", parameters.Select(p => p.ToString())) + ")";

            var methodCall = $"return this._wrapper.{moduleName}.{methodDeclaration.Identifier}(this, {string.Join(", ", parameters.Select(p => p.Identifier.ToString()))});";

            builder.AppendLine(@"
        " + signature + @"
        {
            " + methodCall + @"
        }");
        }

        private StringBuilder GenerateClass(TypeInfo relatedClass)
        {
            var sb = new StringBuilder();

            sb.Append(@"
using System;
using System.Collections.Generic;

namespace SpeedifyCliWrapper.ReturnTypes
{
    public partial class " + relatedClass.Type.Name);

            sb.Append(@"
    {");

            return sb;
        }
        private void CloseClass(StringBuilder generatedClass)
        {
            generatedClass.Append(
@"    }
}");
        }
    }
    public class UnitSyntaxReceiver : ISyntaxReceiver
    {
        public ClassDeclarationSyntax ClassToAugment { get; set; }
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds && cds.Identifier.ValueText == "Units")
            {
                ClassToAugment = cds;
            }
        }
    }
}
