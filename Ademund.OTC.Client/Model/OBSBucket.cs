using System;
using System.Xml.Serialization;

namespace Ademund.OTC.Client.Model
{
    [XmlRoot(ElementName="ListAllMyBucketsResult", Namespace="http://obs.otc.t-systems.com/doc/2016-01-01/")]
    public sealed record OBSBucketsCollectionResponse
    {
        public OBSOwner Owner { get; init; }

        [XmlArrayItem("Bucket")]
        public OBSBucket[] Buckets { get; init; }
    }

    public sealed record OBSOwner
    {
        public string ID { get; init; }
        public string DisplayName { get; init; }
    }

    public sealed record OBSBucket
    {
        public string Name { get; init; }
        public DateTime CreationDate { get; init; }
        public string BucketType { get; init; }
    }

    [XmlRoot(ElementName="ListBucketResult", Namespace="http://obs.otc.t-systems.com/doc/2016-01-01/")]
    public sealed record OBSBucketObjectsResponse
    {
        public string Name { get; init; }
        public string Prefix { get; init; }
        public string Marker { get; init; }
        public int MaxKeys { get; init; }
        public bool IsTruncated { get; init; }

        [XmlElement("Contents")]
        public OBSBucketObject[] Objects { get; init; }
    }

    public sealed record OBSBucketObject
    {
        public string Key { get; init; }
        public DateTime LastModified { get; init; }
        public string ETag { get; init; }
        public long Size { get; init; }
        public OBSOwner Owner { get; init; }
        public string StorageClass { get; init; }
    }

    public static class OBSBucketObjectAcl
    {
        public const string Private = "private";
        public const string PublicRead = "public-read";
        public const string PublicReadWrite = "public-read-write";
        public const string AuthenticatedRead = "authenticated-read";
        public const string BucketOwnerRead = "bucket-owner-read";
        public const string BucketOwnerFullControl = "bucket-owner-full-control";
    }

    public static class OBSBucketObjectStorageClass
    {
        public const string Standard = "STANDARD";
        public const string Warm = "STANDARD_IA";
        public const string Cold = "GLACIER";
    }
}

