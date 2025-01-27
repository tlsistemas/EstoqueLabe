﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace EstoqueLab.Uteis.Swagger.Filters
{
    public class ReplaceFileParamOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasOperationHasFileUploadButton = context.MethodInfo.GetCustomAttribute<AddSwaggerFileUploadButtonAttribute>();

            if (hasOperationHasFileUploadButton == null)
            {
                return;
            }

            RemoveExistingFileParameter(operation.Parameters, hasOperationHasFileUploadButton.ParameterName);

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = hasOperationHasFileUploadButton.ParameterName,
                Required = true,
                In = ParameterLocation.Header,// "formData",
                Description = hasOperationHasFileUploadButton.ParameterName
            });
        }

        private void RemoveExistingFileParameter(IList<OpenApiParameter> operationParameters, String parameterName)
        {
            var parameter = operationParameters.FirstOrDefault(p => p.Name == parameterName);

            if (parameterName != null)
            {
                operationParameters.Remove(parameter);
            }
        }
    }

}
