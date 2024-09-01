namespace ReStack.Common.Constants;

public static class EndPoints
{
    public const string Stack = "api/stack";
    public const string Stack_Add = $"{Stack}";
    public const string Stack_Update = $"{Stack}";
    public const string Stack_Delete = $"{Stack}/{{stackId}}";
    public const string Stack_Get = $"{Stack}/{{stackId}}";
    public const string Stack_GetAll = $"{Stack}";
    public const string Stack_Execute = $"{Stack}/{{stackId}}/execute";
    public const string Stack_ReadFile = $"{Stack}/{{stackId}}/file/read";
    public const string Stack_UploadFile = $"{Stack}/{{stackId}}/file";
    public const string Stack_Cancel = $"{Stack}/{{stackId}}/cancel";

    public const string Job = "api/job";
    public const string Job_Get = $"{Job}/{{jobId}}";
    public const string Job_GetBySequence = $"{Job}/{{sequence}}/stack/{{stackId}}";
    public const string Job_GetAllSkip = $"{Job}/{{stackId}}/list/{{skip}}/{{take}}";
    public const string Job_Delete = $"{Job}/{{jobId}}";
    public const string Job_Cancel = $"{Job}/{{jobId}}/cancel";

    public const string SshKey = "api/sshkey";
    public const string SshKey_Get = $"{SshKey}";
    public const string SshKey_Generate = $"{SshKey}/generate";

    public const string ComponentLibrary = "api/componentlibrary";
    public const string ComponentLibrary_Sync = $"{ComponentLibrary}/sync";
    public const string ComponentLibrary_Compose = $"{ComponentLibrary}/compose";
    public const string ComponentLibrary_Delete = $"{ComponentLibrary}/{{componentLibraryId}}";
    public const string ComponentLibrary_Get = $"{ComponentLibrary}/{{componentLibraryId}}";
    public const string ComponentLibrary_GetAll = $"{ComponentLibrary}";
    public const string ComponentLibrary_GetUsingStacks = $"{ComponentLibrary}/{{componentLibraryId}}/stacks";

    public const string Tag = "api/tag";
    public const string Tag_Add = $"{Tag}";
    public const string Tag_Update = $"{Tag}";
    public const string Tag_Delete = $"{Tag}/{{tagId}}";
    public const string Tag_Get = $"{Tag}/{{tagId}}";
    public const string Tag_GetAll = $"{Tag}";

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
