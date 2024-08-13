using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Speckle.Core.Api;
using Speckle.Core.Api.SubscriptionModels;
using Speckle.Core.Credentials;
using Speckle.Core.Models;
using Speckle.Core.Transports;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Net.Http.Json;

namespace CSMSBE.Core.Helper
{
    public class SpeckleHelper
    {
        public static string _serverURL = "";
        public static string _authToken = "";
        public static Account account = new Account();
        public static void SetupSpeckleAccount(string serverUrl, string authToken)
        {
            _serverURL = serverUrl;
            _authToken = authToken;
            account.token = authToken;
            account.serverInfo = new ServerInfo() { url = serverUrl };
        }

        public static async Task<string> CreateStream(string name, string description, bool isPublic)
        {
            try
            {
                Client client = new Client(account);
                var streamInput = new StreamCreateInput
                {
                    name = name,
                    description = description,
                    isPublic = isPublic
                };
                var streamId = await client.StreamCreate(streamInput);
                return streamId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<bool> UpdateStream(string id, string name, bool isPublic, string description)
        {
            try
            {
                Client client = new Client(account);
                var streamUpdateInput = new StreamUpdateInput()
                {
                    id = id,
                    name = name,
                    description = description,
                    isPublic = isPublic
                };
                var streamUpdateResult = await client.StreamUpdate(streamUpdateInput);
                return streamUpdateResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<Speckle.Core.Api.Stream> GetStream(string streamId)
        {
            try
            {
                Client client = new Client(account);
                var stream = await client.StreamGet(streamId);
                return stream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> CreateBranch(string branchName, string streamId, string description)
        {
            try
            {
                Client client = new Client(account);
                var branchInput = new BranchCreateInput
                {
                    name = branchName,
                    description = description,
                    streamId = streamId
                };
                var branchId = await client.BranchCreate(branchInput);
                return branchId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<bool> UpdateBranch(string id, string name, string streamId, string description)
        {
            try
            {
                Client client = new Client(account);
                var branchUpdateInput = new BranchUpdateInput()
                {
                    id = id,
                    name = name,
                    description = description,
                    streamId = streamId
                };
                var branchUpdateResult = await client.BranchUpdate(branchUpdateInput);
                return branchUpdateResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<Branch> GetBranch(string branchName, string streamId)
        {
            try
            {
                Client client = new Client(account);
                var branch = await client.BranchGet(streamId, branchName);
                return branch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<Commit> GetCommit(string streamId, string commitId)
        {
            try
            {
                Client client = new Client(account);
                var commit = await client.CommitGet(streamId, commitId);
                return commit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<bool> UpdateCommit(string id, string streamId, string message)
        {
            try
            {
                Client client = new Client(account);
                var commitUpdateInput = new CommitUpdateInput()
                {
                    id = id,
                    message = message,
                    streamId = streamId
                };
                var commitUpdateResult = await client.CommitUpdate(commitUpdateInput);
                return commitUpdateResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<bool> DeleteStream(string streamId)
        {
            try
            {
                Client client = new Client(account);
                var isStreamDeleted = await client.StreamDelete(streamId);
                return isStreamDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<bool> DeleteBranch(string branchId, string streamId)
        {
            try
            {
                Client client = new Client(account);
                var input = new BranchDeleteInput(){
                    id = branchId,
                    streamId = streamId
                };
                var isBranchDeleted = await client.BranchDelete(input);
                return isBranchDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<bool> DeleteCommit(string streamId, string commitId)
        {
            try
            {
                Client client = new Client(account);
                var input = new CommitDeleteInput()
                {
                    id = commitId,
                    streamId = streamId
                };
                var isCommitDeleted = await client.CommitDelete(input);
                return isCommitDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<SpeckleUploadResult> UploadFileIFC(string streamId, string branchName, string ifcFilePath)
        {
            try
            {
                var commitInfoTask = new TaskCompletionSource<CommitInfo>();

                void OnCreateCommitHandler(object sender, CommitInfo e)
                {
                    commitInfoTask.TrySetResult(e);
                }
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
                    using (var form = new MultipartFormDataContent())
                    {
                        using (var fileStream = new FileStream(ifcFilePath, FileMode.Open, FileAccess.Read))
                        {
                            var streamContent = new StreamContent(fileStream);
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            form.Add(streamContent, "file", Path.GetFileName(ifcFilePath));
                            string s = $"{_serverURL}api/file/autodetect/{streamId}/{branchName}";
                            Client speckleClient = new Client(account);
                            speckleClient.SubscribeCommitCreated(streamId);
                            speckleClient.OnCommitCreated += OnCreateCommitHandler;
                            var response = await client.PostAsync(s, form);
                            response.EnsureSuccessStatusCode();
                            var commitInfo = await commitInfoTask.Task;
                            speckleClient.OnCommitCreated -= OnCreateCommitHandler;
                            return new SpeckleUploadResult(commitInfo.objectId, commitInfo.id, streamId,branchName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<Speckle.Core.Api.Comments> GetComment(string streamId, int limit)
        {
            try
            {
                Client client = new Client(account);
                var comment = await client.StreamGetComments(streamId, limit);
                return comment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> CreateComment(string streamId, string text, string resourceId, string resourceType, string? parentId = null)
        {
            try
            {
                var request = new
                {
                    query = @"
            mutation CreateComment($streamId: String!, $text: String!, $resourceId: String!, $resourceType: String!, $parentId: String) {
                commentCreate(streamId: $streamId, input: { text: $text, resources: [{ resourceId: $resourceId, resourceType: $resourceType }], parent: $parentId }) {
                    id
                    rawText
                }
            }",
                    variables = new
                    {
                        streamId,
                        text,
                        resourceId,
                        resourceType,
                        parentId
                    }
                };

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
                var response = await client.PostAsJsonAsync($"{_serverURL}/graphql", request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return result.data.commentCreate.id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> UpdateComment(string streamId, string commentId, string newText)
        {
            try
            {
                var request = new
                {
                    query = @"
            mutation UpdateComment($streamId: String!, $commentId: String!, $newText: String!) {
                commentUpdate(streamId: $streamId, commentId: $commentId, text: $newText) {
                    id
                    rawText
                }
            }",
                    variables = new
                    {
                        streamId,
                        commentId,
                        newText
                    }
                };

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
                var response = await client.PostAsJsonAsync($"{_serverURL}/graphql", request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return result.data.commentUpdate.id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<bool> DeleteComment(string streamId, string commentId)
        {
            try
            {
                var request = new
                {
                    query = @"
                mutation DeleteComment($streamId: String!, $commentId: String!) {
                    commentDelete(streamId: $streamId, commentId: $commentId)
                }",
                    variables = new
                    {
                        streamId,
                        commentId
                    }
                };

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
                var response = await client.PostAsJsonAsync($"{_serverURL}/graphql", request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return result.data.commentDelete;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class SpeckleUploadResult
    {
        public string ObjectId { get; set; }
        public string CommitId { get; set; }
        public string StreamId { get; set; }
        public string BranchName { get; set; }

        public SpeckleUploadResult(string objectId, string commitId, string streamId, string branchName)
        {
            ObjectId = objectId;
            CommitId = commitId;
            StreamId = streamId;
            BranchName = branchName;
        }
    }
}
