using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Climp.Commands.Search;
using Climp.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Climp
{
    public sealed class MediaIndex
    {
        private const string MetadataPropertyName = "metadata";
        private const string IndexPropertyName = "index";
        private static readonly Version _indexVersion = new Version(1, 0);

        private readonly FileInfo _indexFile;
        private readonly Config _config;

        public MediaIndex(Config config, FileInfo indexFile)
            => (_config, _indexFile) = (config, indexFile);

        public IEnumerable<MediaFile> SearchForFiles(SearchPredicate searchPredicate)
            => _GetMediaFiles()
                    .Where(mediaFile => searchPredicate.GetRank(mediaFile) > 0)
                    .OrderByDescending(searchPredicate.GetRank);

        public void Refresh(Context context)
        {
            context.Output.WriteLine("Checking index");
            var indexMetadata = _GetIndexMetadata();
            if (indexMetadata is null || _indexVersion != indexMetadata.Version || _config.MediaDirectories.Any(mediaDirectory => mediaDirectory.LastWriteTime.ToUniversalTime() > indexMetadata.CreateDate))
            {
                context.Output.WriteLine("Index is outdated, refreshing");

                var stopwatch = Stopwatch.StartNew();

                indexMetadata = new MediaIndexMetadata
                {
                    Version = _indexVersion,
                    CreateDate = DateTime.UtcNow
                };

                using (var indexFileStream = new FileStream(_indexFile.FullName, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (var indexFileStreamWriter = new StreamWriter(indexFileStream))
                using (var indexJsonWriter = new JsonTextWriter(indexFileStreamWriter))
                {
                    var jsonSerializer = _GetJsonSerializer();

                    indexJsonWriter.WriteStartObject();
                    indexJsonWriter.WritePropertyName(MetadataPropertyName);
                    jsonSerializer.Serialize(indexJsonWriter, indexMetadata);

                    indexJsonWriter.WritePropertyName(IndexPropertyName);
                    indexJsonWriter.WriteStartArray();

                    foreach (var file in _config.MediaDirectories.SelectMany(mediaDirecotry => mediaDirecotry.EnumerateFiles("*.m4a", SearchOption.AllDirectories)))
                    {
                        var tagFile = TagLib.File.Create(file.FullName, TagLib.ReadStyle.PictureLazy);
                        var mediaFile = new MediaFile
                        {
                            File = file,
                            Title = tagFile.Tag.Title,
                            Artists = tagFile.Tag.AlbumArtists
                        };
                        jsonSerializer.Serialize(indexJsonWriter, mediaFile);
                    }

                    indexJsonWriter.WriteEndArray();

                    indexJsonWriter.WriteEndObject();
                }
                stopwatch.Stop();

                context.Output.WriteLine($"Index refresh completed, took {stopwatch.Elapsed}.");
            }
            else
                context.Output.WriteLine("Index is up to date");
        }

        private MediaIndexMetadata _GetIndexMetadata()
        {
            if (_indexFile.Exists)
                using (var indexFileStream = new FileStream(_indexFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var indexFileStreamReader = new StreamReader(indexFileStream))
                using (var indexJsonReader = new JsonTextReader(indexFileStreamReader))
                {
                    while (indexJsonReader.Read() && indexJsonReader.TokenType != JsonToken.PropertyName && indexJsonReader.Value as string != MetadataPropertyName)
                        ;
                    if (indexJsonReader.Read() && indexJsonReader.TokenType == JsonToken.StartObject)
                        return JObject.ReadFrom(indexJsonReader).ToObject<MediaIndexMetadata>();
                    else
                        return null;
                }
            else
                return null;
        }

        private IEnumerable<MediaFile> _GetMediaFiles()
        {
            using (var indexFileStream = new FileStream(_indexFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var indexFileStreamReader = new StreamReader(indexFileStream))
            using (var indexJsonReader = new JsonTextReader(indexFileStreamReader))
            {
                while (indexJsonReader.Read() && (indexJsonReader.TokenType != JsonToken.PropertyName || indexJsonReader.Value as string != IndexPropertyName))
                    ;
                if (indexJsonReader.Read() && indexJsonReader.TokenType == JsonToken.StartArray)
                {
                    var jsonSerializer = _GetJsonSerializer();
                    while (indexJsonReader.Read() && indexJsonReader.TokenType == JsonToken.StartObject)
                        yield return jsonSerializer.Deserialize<MediaFile>(indexJsonReader);
                }
            }
        }

        private static JsonSerializer _GetJsonSerializer()
            => new JsonSerializer
            {
                Converters = { new FileInfoJsonConverter() },
                Culture = CultureInfo.InvariantCulture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented
            };
    }
}