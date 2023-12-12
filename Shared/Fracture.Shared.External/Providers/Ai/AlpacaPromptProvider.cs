namespace Fracture.Shared.External.Providers.Ai
{
    public class AlpacaPromptProvider : IPromptTemplateProvider
    {
        public string GetPrompt(PromptTemplate template)
        {
            return $"""
                {template.System}

                ### Instruction:
                {template.Input}

                ### Response {template.ResponseParams}:
                {template.Response}
                """;
        }
    }
}
