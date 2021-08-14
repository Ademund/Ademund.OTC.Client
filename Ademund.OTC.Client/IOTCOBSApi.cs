using Ademund.OTC.Client.Model;
using RestEase;
using RestEase.Implementation;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ademund.OTC.Client
{
    public interface IOTCOBSApi : IOTCApiBase
    {
        IRequester Requester { get; }

        [Get("/")]
        Task<OBSBucketsCollectionResponse> GetBucketsAsync();

        [Get("/{bucketName}")]
        Task<OBSBucketObjectsResponse> GetBucketObjectsAsync(
            [Path("bucketName")] string bucketName,
            [Query("prefix")] string prefix = null,
            [Query("marker")] string marker = null,
            [Query("max-keys")] int maxKeys = 1000,
            [Query("delimiter")] string delimiter = null,
            CancellationToken cancellationToken = default);

        [Put("/{objectName}")]
        Task<HttpResponseMessage> UploadBucketObjectAsync(
            [Path("objectName")] string objectName,
            [Body] Stream objectData,
            [Header("Content-Type")] string contentType = "application/octet-stream",
            [Header("x-amz-acl")] string acl = OBSBucketObjectAcl.Private,
            [Header("x-amz-storage-class")] string storageClass = OBSBucketObjectStorageClass.Standard,
            [Header("x-amz-website-redirect-location")] string websiteRedirectLocation = null,
            [Header("x-amz-server-side-encryption")] string serverSideEncryption = null,
            [Header("x-amz-server-side-encryption-aws-kms-key-id")] string serverSideEncryptionAwsKmsKeyId = null,
            [Header("x-amz-server-side-encryption-customer-algorithm")] string serverSideEncryptionCustomerAlgorithm = null,
            [Header("x-amz-server-side-encryption-customer-key")] string serverSideEncryptionCustomerKey = null,
            [Header("x-amz-server-side-encryption-customer-key-MD5")] string serverSideEncryptionCustomerKeyMD5 = null,
            [Header("x-amz-security-token")] string securityToken = null,
            [Header("Content-MD5")] string contentMD5 = null,
            CancellationToken cancellationToken = default);
    }

    public static class IOTCOBSApiExtensions
    {
        private static readonly CustomXmlResponseDeserializer responseDeserializer = new();

        public static async Task<OBSBucketObjectsResponse> UploadBucketObjectWithCustomMeta(this IOTCOBSApi api,
            string objectName,
            Stream objectData,
            string contentType = "application/octet-stream",
            string acl = OBSBucketObjectAcl.Private,
            string storageClass = OBSBucketObjectStorageClass.Standard,
            string websiteRedirectLocation = null,
            string serverSideEncryption = null,
            string serverSideEncryptionAwsKmsKeyId = null,
            string serverSideEncryptionCustomerAlgorithm = null,
            string serverSideEncryptionCustomerKey = null,
            string serverSideEncryptionCustomerKeyMD5 = null,
            string securityToken = null,
            string contentMD5 = null,
            Dictionary<string, string> customAmzMeta = null,
            CancellationToken cancellationToken = default)
        {
            var requestInfo = new RequestInfo(HttpMethod.Put, $"/{objectName}") {
                CancellationToken = cancellationToken
            };
            requestInfo.AddHeaderParameter("Content-Type", contentType);
            if (!string.IsNullOrWhiteSpace(acl))
                requestInfo.AddHeaderParameter("x-amz-acl", acl);
            if (!string.IsNullOrWhiteSpace(storageClass))
                requestInfo.AddHeaderParameter("x-amz-storage-class", storageClass);
            if (!string.IsNullOrWhiteSpace(websiteRedirectLocation))
                requestInfo.AddHeaderParameter("x-amz-website-redirect-location", websiteRedirectLocation);
            if (!string.IsNullOrWhiteSpace(serverSideEncryption))
                requestInfo.AddHeaderParameter("x-amz-server-side-encryption", serverSideEncryption);
            if (!string.IsNullOrWhiteSpace(serverSideEncryptionAwsKmsKeyId))
                requestInfo.AddHeaderParameter("x-amz-server-side-encryption-aws-kms-key-id", serverSideEncryptionAwsKmsKeyId);
            if (!string.IsNullOrWhiteSpace(serverSideEncryptionCustomerAlgorithm))
                requestInfo.AddHeaderParameter("x-amz-server-side-encryption-customer-algorithm", serverSideEncryptionCustomerAlgorithm);
            if (!string.IsNullOrWhiteSpace(serverSideEncryptionCustomerKey))
                requestInfo.AddHeaderParameter("x-amz-server-side-encryption-customer-key", serverSideEncryptionCustomerKey);
            if (!string.IsNullOrWhiteSpace(serverSideEncryptionCustomerKeyMD5))
                requestInfo.AddHeaderParameter("x-amz-server-side-encryption-customer-key-MD5", serverSideEncryptionCustomerKeyMD5);
            if (!string.IsNullOrWhiteSpace(securityToken))
                requestInfo.AddHeaderParameter("x-amz-security-token", securityToken);
            if (!string.IsNullOrWhiteSpace(contentMD5))
                requestInfo.AddHeaderParameter("Content-MD5", contentMD5);
            foreach (var meta in customAmzMeta ?? new Dictionary<string, string>())
            {
                requestInfo.AddHeaderParameter(meta.Key, meta.Value);
            }

            requestInfo.SetBodyParameterInfo(BodySerializationMethod.Default, objectData);
            using (var response = await api.Requester.RequestWithResponseMessageAsync(requestInfo).ConfigureAwait(false))
            {
                string content = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseDeserializer.Deserialize<OBSBucketObjectsResponse>(content, response, new ResponseDeserializerInfo(requestInfo));
            }
        }
    }
}
