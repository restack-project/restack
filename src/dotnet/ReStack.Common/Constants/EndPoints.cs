namespace ReStack.Common.Constants;

public static class EndPoints
{
    public const string Stack = "api/stack";
    public const string Stack_Add = "api/stack";
    public const string Stack_Update = "api/stack";
    public const string Stack_Delete = "api/stack/{stackId}";
    public const string Stack_Get = "api/stack/{stackId}";
    public const string Stack_GetAll = "api/stack";
    public const string Stack_Execute = "api/stack/{stackId}/execute";
    public const string Stack_ReadFile = "api/stack/{stackId}/file/read";
    public const string Stack_UploadFile = "api/stack/{stackId}/file";
    public const string Stack_Cancel = "api/stack/{stackId}/cancel";

    public const string Job = "api/job";
    public const string Job_Get = "api/job/{jobId}";
    public const string Job_GetBySequence = "api/job/{sequence}/stack/{stackId}";
    public const string Job_GetAllSkip = "api/job/{stackId}/list/{skip}/{take}";
    public const string Job_Delete = "api/job/{jobId}";
    public const string Job_Cancel = "api/job/{jobId}/cancel";

    public const string SshKey = "api/sshkey";
    public const string SshKey_Get = "api/sshkey";
    public const string SshKey_Generate = "api/sshkey/generate";

    public const string ComponentLibrary = "api/componentlibrary";
    public const string ComponentLibrary_Sync = "api/componentlibrary/sync";
    public const string ComponentLibrary_Compose = "api/componentlibrary/compose";
    public const string ComponentLibrary_Delete = "api/componentlibrary/{componentLibraryId}";
    public const string ComponentLibrary_Get = "api/componentlibrary/{componentLibraryId}";
    public const string ComponentLibrary_GetAll = "api/componentlibrary";
    public const string ComponentLibrary_GetUsingStacks = "api/componentlibrary/{componentLibraryId}/stacks";

    public static string Resolve(this string url, string key, object value)
    {
        url = url.Replace($"{{{key}}}", value.ToString());

        return url;
    }

    public static string AddRouteParameter(this string url, object parameter)
    {
        url = $"{url}/{parameter}";

        return url;
    }

    public static string AddQueryString(this string url, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return url;
        }

        if (!query.StartsWith("?"))
        {
            query = "?" + query;
        }

        url = $"{url}{query}";

        return url;
    }
}
