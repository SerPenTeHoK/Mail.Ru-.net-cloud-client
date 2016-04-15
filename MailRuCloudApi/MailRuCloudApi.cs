﻿//-----------------------------------------------------------------------
// <created file="MailRuCloudApi.cs">
//     Mail.ru cloud client created in 2016.
// </created>
// <author>Korolev Erast.</author>
//-----------------------------------------------------------------------

namespace MailRuCloudApi
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Cloud client.
    /// </summary>
    public class MailRuCloud
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MailRuCloud" /> class. Do not forget to set Account property before using any API functions.
        /// </summary>
        public MailRuCloud()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailRuCloud" /> class.
        /// </summary>
        /// <param name="login">Login name as the email.</param>
        /// <param name="password">Password, associated with this email.</param>
        public MailRuCloud(string login, string password)
        {
            this.Account = new Account(login, password);
            if (!this.Account.Login())
            {
                throw new Exception("Auth token has't been retrieved.");
            }
        }

        /// <summary>
        /// Changing progress event, works only for GetFileAsync and UploadFileAsync functions.
        /// </summary>
        public event ProgressChangedEventHandler ChangingProgressEvent;

        /// <summary>
        /// Gets or sets account to connect with cloud.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Copying folder in another space on the server.
        /// </summary>
        /// <param name="folder">Folder info to copying.</param>
        /// <param name="destinationEntry">Destination entry on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Copy(Folder folder, Entry destinationEntry)
        {
            return await this.Copy(folder, destinationEntry.FullPath);
        }

        /// <summary>
        /// Copying folder in another space on the server.
        /// </summary>
        /// <param name="folder">Folder info to copying.</param>
        /// <param name="destinationFolder">Destination folder on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Copy(Folder folder, Folder destinationFolder)
        {
            return await this.Copy(folder, destinationFolder.FulPath);
        }

        /// <summary>
        /// Copying folder in another space on the server.
        /// </summary>
        /// <param name="folder">Folder info to copying.</param>
        /// <param name="destinationPath">Destination path on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Copy(Folder folder, string destinationPath)
        {
            return await this.MoveOrCopy(folder.Name, folder.FulPath, destinationPath, false);
        }

        /// <summary>
        /// Copying file in another space on the server.
        /// </summary>
        /// <param name="file">File info to copying.</param>
        /// <param name="destinationEntry">Destination entry on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Copy(File file, Entry destinationEntry)
        {
            return await this.Copy(file, destinationEntry.FullPath);
        }

        /// <summary>
        /// Copying file in another space on the server.
        /// </summary>
        /// <param name="file">File info to copying.</param>
        /// <param name="destinationFolder">Destination folder on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Copy(File file, Folder destinationFolder)
        {
            return await this.Copy(file, destinationFolder.FulPath);
        }

        /// <summary>
        /// Copying file in another space on the server.
        /// </summary>
        /// <param name="file">File info to copying.</param>
        /// <param name="destinationPath">Destination path on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Copy(File file, string destinationPath)
        {
            return await this.MoveOrCopy(file.Name, file.FulPath, destinationPath, false);
        }

        /// <summary>
        /// Rename folder on the server.
        /// </summary>
        /// <param name="folder">Source folder info.</param>
        /// <param name="newFileName">New folder name.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Rename(Folder folder, string newFileName)
        {
            return await this.Rename(folder.Name, folder.FulPath, newFileName);
        }

        /// <summary>
        /// Rename file on the server.
        /// </summary>
        /// <param name="file">Source file info.</param>
        /// <param name="newFileName">New file name.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Rename(File file, string newFileName)
        {
            return await this.Rename(file.Name, file.FulPath, newFileName);
        }

        /// <summary>
        /// Move folder in another space on the server.
        /// </summary>
        /// <param name="folder">Folder info to moving.</param>
        /// <param name="destinationFolder">Destination folder on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Move(Folder folder, Folder destinationFolder)
        {
            return await this.Move(folder, destinationFolder.FulPath);
        }

        /// <summary>
        /// Move folder in another space on the server.
        /// </summary>
        /// <param name="folder">Folder info to moving.</param>
        /// <param name="destinationEntry">Destination entry on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Move(Folder folder, Entry destinationEntry)
        {
            return await this.Move(folder, destinationEntry.FullPath);
        }

        /// <summary>
        /// Move folder in another space on the server.
        /// </summary>
        /// <param name="folder">Folder info to move.</param>
        /// <param name="destinationPath">Destination path on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Move(Folder folder, string destinationPath)
        {
            return await this.MoveOrCopy(folder.Name, folder.FulPath, destinationPath, true);
        }

        /// <summary>
        /// Move file in another space on the server.
        /// </summary>
        /// <param name="file">File info to move.</param>
        /// <param name="destinationEntry">Destination entry on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Move(File file, Entry destinationEntry)
        {
            return await this.Move(file, destinationEntry.FullPath);
        }

        /// <summary>
        /// Move file in another space on the server.
        /// </summary>
        /// <param name="file">File info to move.</param>
        /// <param name="destinationFolder">Destination folder on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Move(File file, Folder destinationFolder)
        {
            return await this.Move(file, destinationFolder.FulPath);
        }

        /// <summary>
        /// Move file in another space on the server.
        /// </summary>
        /// <param name="file">File info to move.</param>
        /// <param name="destinationPath">Destination path on the server.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Move(File file, string destinationPath)
        {
            return await this.MoveOrCopy(file.Name, file.FulPath, destinationPath, true);
        }

        /// <summary>
        /// Create folder on the server.
        /// </summary>
        /// <param name="name">New path name.</param>
        /// <param name="createIn">Destination path.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> CreateFolder(string name, string createIn)
        {
            return await this.AddFileInCloud(new File()
            {
                Name = name,
                FulPath = createIn.EndsWith("/") ? createIn + name : createIn + "/" + name,
                Hash = null,
                Size = new FileSize()
                {
                    DefaultValue = 0
                }
            });
        }

        /// <summary>
        /// Remove the file on server.
        /// </summary>
        /// <param name="file">File info.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Remove(File file)
        {
            return await this.Remove(file.Name, file.FulPath);
        }

        /// <summary>
        /// Remove the folder on server.
        /// </summary>
        /// <param name="folder">Folder info.</param>
        /// <returns>True or false operation result.</returns>
        public async Task<bool> Remove(Folder folder)
        {
            return await this.Remove(folder.Name, folder.FulPath);
        }

        /// <summary>
        /// Get direct link by public file URL. Direct link works only for one session.
        /// </summary>
        /// <param name="publishLink">Public file link.</param>
        /// <returns>Direct link.</returns>
        public async Task<string> GetPublishDirectLink(string publishLink)
        {
            CookieContainer cookie = null;
            var shard = await this.GetShardInfo(ShardType.WeblinkGet, true, cookie);
            var addFileRequest = Encoding.UTF8.GetBytes(string.Format("api={0}", 2));

            var url = new Uri(string.Format("{0}/api/v2/tokens/download", ConstSettings.CloudDomain));
            var request = (HttpWebRequest)WebRequest.Create(url.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = cookie;
            request.Method = "POST";
            request.ContentLength = addFileRequest.LongLength;
            request.Referer = publishLink;
            request.Headers.Add("Origin", ConstSettings.CloudDomain);
            request.Host = url.Host;
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "application/json";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetRequestStream, asyncResult => request.EndGetRequestStream(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var s = t.Result)
                {
                    s.Write(addFileRequest, 0, addFileRequest.Length);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception();
                        }

                        var token = (string)JsonParser.Parse(this.ReadResponseAsText(response), PObject.Token);
                        return string.Format("{0}/{1}?key={2}", shard.Url, publishLink.Replace(ConstSettings.PublishFileLink, string.Empty), token);
                    }
                }
            });
        }

        /// <summary>
        /// Remove the file from public access.
        /// </summary>
        /// <param name="file">File info.</param>
        /// <returns>True or false result of the operation.</returns>
        public async Task<bool> UnpublishLink(File file)
        {
            return (await this.PublishUnpulishLink(file.Name, file.FulPath, false, file.PublicLink)).ToUpper() == file.FulPath.ToUpper();
        }

        /// <summary>
        /// Remove the folder from public access.
        /// </summary>
        /// <param name="folder">Folder info.</param>
        /// <returns>True or false result of the operation.</returns>
        public async Task<bool> UnpublishLink(Folder folder)
        {
            return (await this.PublishUnpulishLink(folder.Name, folder.FulPath, false, folder.PublicLink)).ToUpper() == folder.FulPath.ToUpper();
        }

        /// <summary>
        /// Get public access to the file.
        /// </summary>
        /// <param name="file">File info.</param>
        /// <returns>Public file link.</returns>
        public async Task<string> GetPublishLink(File file)
        {
            return await this.PublishUnpulishLink(file.Name, file.FulPath, true, null);
        }

        /// <summary>
        /// Get public access to the folder.
        /// </summary>
        /// <param name="folder">Folder info.</param>
        /// <returns>Public folder link.</returns>
        public async Task<string> GetPublishLink(Folder folder)
        {
            return await this.PublishUnpulishLink(folder.Name, folder.FulPath, true, null);
        }

        /// <summary>
        /// Get list of files and folders from account.
        /// </summary>
        /// <param name="folder">Folder info.</param>
        /// <returns>List of the items.</returns>
        public async Task<Entry> GetItems(Folder folder)
        {
            return await this.GetItems(folder.FulPath);
        }

        /// <summary>
        /// Get list of files and folders from account.
        /// </summary>
        /// <param name="path">Path in the cloud to return the list of the items.</param>
        /// <returns>List of the items.</returns>
        public async Task<Entry> GetItems(string path)
        {
            this.CheckAuth();
            if (string.IsNullOrEmpty(path))
            {
                path = "/";
            }

            var uri = new Uri(string.Format("{0}/api/v2/folder?token={1}&home={2}", ConstSettings.CloudDomain, this.Account.AuthToken, HttpUtility.UrlEncode(path)));
            var request = (HttpWebRequest)WebRequest.Create(uri.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "GET";
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "application/json";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetResponse, asyncResult => request.EndGetResponse(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var response = t.Result as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return (Entry)JsonParser.Parse(this.ReadResponseAsText(response), PObject.Entry);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            });
        }

        /// <summary>
        /// Download file asynchronously, if not use async await will be use synchronously operation.
        /// </summary>
        /// <param name="file">File info.</param>
        /// <param name="destinationPath">Destination path on the file system.</param>
        /// <returns>True or false result of the operation.</returns>
        public async Task<bool> GetFile(File file, string destinationPath)
        {
            var taskAction = new object[] { file, destinationPath };
            return await Task.Factory.StartNew(
                (action) =>
            {
                var param = action as object[];
                return (bool)this.GetFile((param[0] as File).FulPath, (param[0] as File).Name, param[1] as string, (param[0] as File).Size.DefaultValue).Result;
            },
            taskAction);
        }

        /// <summary>
        /// Download file asynchronously, if not use async await will be use synchronously operation.
        /// </summary>
        /// <param name="file">File info.</param>
        /// <returns>File as byte array.</returns>
        public async Task<byte[]> GetFile(File file)
        {
            var taskAction = new object[] { file };
            return await Task.Factory.StartNew(
                (action) =>
            {
                var param = action as object[];
                var fileInfo = param[0] as File;
                return (byte[])this.GetFile(fileInfo.FulPath, fileInfo.Name, null, fileInfo.Size.DefaultValue).Result;
            },
            taskAction);
        }

        /// <summary>
        /// Upload file on the server asynchronously, if not use async await will be use synchronously operation.
        /// </summary>
        /// <param name="file">File info.</param>
        /// <param name="destinationPath">Destination path on the server.</param>
        /// <returns>True or false result of the operation.</returns>
        public async Task<bool> UploadFile(FileInfo file, string destinationPath)
        {
            destinationPath = destinationPath.EndsWith("/") ? destinationPath : destinationPath + "/";
            var maxFileSize = 2L * 1024L * 1024L * 1024L;
            if (file.Length > maxFileSize)
            {
                throw new OverflowException("Not supported file size.", new Exception(string.Format("The maximum file size is {0} byte. Currently file size is {1} byte.", maxFileSize, file.Length)));
            }

            this.CheckAuth();
            var shard = await this.GetShardInfo(ShardType.Upload);
            var boundary = Guid.NewGuid();

            //// Boundary request building.
            var boundaryBuilder = new StringBuilder();
            boundaryBuilder.AppendFormat("------{0}\r\n", boundary);
            boundaryBuilder.AppendFormat("Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\n", file.Name);
            boundaryBuilder.AppendFormat("Content-Type: {0}\r\n\r\n", ConstSettings.GetContentType(file.Extension));

            var endBoundaryBuilder = new StringBuilder();
            endBoundaryBuilder.AppendFormat("\r\n------{0}--\r\n", boundary);

            var endBoundaryRequest = Encoding.UTF8.GetBytes(endBoundaryBuilder.ToString());
            var boundaryRequest = Encoding.UTF8.GetBytes(boundaryBuilder.ToString());

            var url = new Uri(string.Format("{0}?cloud_domain=2&{1}", shard.Url, this.Account.LoginName));
            var request = (HttpWebRequest)WebRequest.Create(url.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "POST";
            request.ContentLength = file.Length + boundaryRequest.LongLength + endBoundaryRequest.LongLength;
            request.Referer = string.Format("{0}/home{1}", ConstSettings.CloudDomain, HttpUtility.UrlEncode(destinationPath));
            request.Headers.Add("Origin", ConstSettings.CloudDomain);
            request.Host = url.Host;
            request.ContentType = string.Format("multipart/form-data; boundary=----{0}", boundary.ToString());
            request.Accept = "*/*";
            request.UserAgent = ConstSettings.UserAgent;
            request.AllowWriteStreamBuffering = false;

            var task = Task.Factory.FromAsync(request.BeginGetRequestStream, asyncResult => request.EndGetRequestStream(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                try
                {
                    using (var s = t.Result)
                    {
                        this.WriteBytesInStream(boundaryRequest, s);
                        this.WriteBytesInStream(file, s, true, OperationType.Upload);
                        this.WriteBytesInStream(endBoundaryRequest, s);

                        using (var response = (HttpWebResponse)request.GetResponse())
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var resp = ReadResponseAsText(response).Split(new char[] { ';' });
                                var hashResult = resp[0];
                                var sizeResult = long.Parse(resp[1].Replace("\r\n", string.Empty));

                                return this.AddFileInCloud(new File()
                                {
                                    Name = file.Name,
                                    FulPath = HttpUtility.UrlDecode(destinationPath) + file.Name,
                                    Hash = hashResult,
                                    Size = new FileSize()
                                    {
                                        DefaultValue = sizeResult
                                    }
                                }).Result;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    }
                }
                finally
                {
                    if (t.Result != null)
                    {
                        t.Result.Dispose();
                    }
                }
            });
        }

        /// <summary>
        /// Read web response as text.
        /// </summary>
        /// <param name="resp">Web response.</param>
        /// <returns>Converted text.</returns>
        internal string ReadResponseAsText(WebResponse resp)
        {
            using (var stream = new MemoryStream())
            {
                this.ReadResponseAsByte(resp, stream);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Read web response as byte array.
        /// </summary>
        /// <param name="resp">Web response.</param>
        /// <param name="outputStream">Output stream to writing the response.</param>
        /// <param name="contentLength">Length of the stream.</param>
        /// <param name="operation">Currently operation type.</param>
        internal void ReadResponseAsByte(WebResponse resp, Stream outputStream = null, long contentLength = 0, OperationType operation = OperationType.None)
        {
            if (contentLength != 0)
            {
                this.OnChangedProgressPercent(new ProgressChangedEventArgs(
                                0,
                                new ProgressChangeTaskState()
                                {
                                    Type = operation,
                                    TotalBytes = new FileSize()
                                    {
                                        DefaultValue = contentLength
                                    },
                                    BytesInProgress = new FileSize()
                                    {
                                        DefaultValue = 0L
                                    }
                                }));
            }

            int bufSizeChunk = 30000;
            int totalBufSize = bufSizeChunk;
            byte[] fileBytes = new byte[totalBufSize];
            double percentComplete = 0;

            int totalBytesRead = 0;

            using (var reader = new BinaryReader(resp.GetResponseStream()))
            {
                int bytesRead = 0;
                while ((bytesRead = reader.Read(fileBytes, totalBytesRead, totalBufSize - totalBytesRead)) > 0)
                {
                    if (outputStream != null)
                    {
                        outputStream.Write(fileBytes, totalBytesRead, bytesRead);
                    }

                    totalBytesRead += bytesRead;

                    if ((totalBufSize - totalBytesRead) == 0)
                    {
                        totalBufSize += bufSizeChunk;
                        Array.Resize(ref fileBytes, totalBufSize);
                    }

                    if (contentLength != 0 && contentLength >= totalBytesRead)
                    {
                        var tempPercentComplete = 100.0 * (double)totalBytesRead / (double)contentLength;
                        if (tempPercentComplete - percentComplete >= 1)
                        {
                            percentComplete = tempPercentComplete;
                            this.OnChangedProgressPercent(new ProgressChangedEventArgs(
                                (int)percentComplete,
                                new ProgressChangeTaskState()
                                {
                                    Type = operation,
                                    TotalBytes = new FileSize()
                                    {
                                        DefaultValue = contentLength
                                    },
                                    BytesInProgress = new FileSize()
                                    {
                                        DefaultValue = totalBytesRead
                                    }
                                }));
                        }
                    }
                }

                if (contentLength != 0)
                {
                    this.OnChangedProgressPercent(new ProgressChangedEventArgs(
                                100,
                                new ProgressChangeTaskState()
                                {
                                    Type = operation,
                                    TotalBytes = new FileSize()
                                    {
                                        DefaultValue = contentLength
                                    },
                                    BytesInProgress = new FileSize()
                                    {
                                        DefaultValue = totalBytesRead
                                    }
                                }));
                }
            }
        }

        /// <summary>
        /// Function to set data for ChangingProgressEvent.
        /// </summary>
        /// <param name="e">Progress changed argument.</param>
        protected virtual void OnChangedProgressPercent(ProgressChangedEventArgs e)
        {
            if (this.ChangingProgressEvent != null)
            {
                this.ChangingProgressEvent(this, e);
            }
        }

        /// <summary>
        /// Download file asynchronously, if not use async await will be use synchronously operation.
        /// </summary>
        /// <param name="sourceFullFilePath">Full file path on the server.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="destinationPath">Destination full file path on the file system.</param>
        /// <param name="contentLength">File length.</param>
        /// <returns>File as byte array.</returns>
        private async Task<object> GetFile(string sourceFullFilePath, string fileName, string destinationPath, long contentLength = 0)
        {
            this.CheckAuth();
            var shard = await this.GetShardInfo(ShardType.Get);
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", shard.Url, sourceFullFilePath.TrimStart('/')));
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "GET";
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = ConstSettings.DefaultAcceptType;
            request.UserAgent = ConstSettings.UserAgent;
            request.AllowReadStreamBuffering = false;

            var task = Task.Factory.FromAsync(request.BeginGetResponse, asyncResult => request.EndGetResponse(asyncResult), (object)null);
            destinationPath = destinationPath == null || destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";
            return await task.ContinueWith((t) =>
            {
                if (destinationPath != null && fileName != null)
                {
                    using (var fileStream = System.IO.File.Create(destinationPath + fileName))
                    {
                        ReadResponseAsByte(t.Result, fileStream, contentLength, OperationType.Download);
                        return fileStream.Length > 0 as object;
                    }
                }
                else
                {
                    var stream = new MemoryStream();
                    ReadResponseAsByte(t.Result, stream, contentLength, OperationType.Download);
                    return stream.ToArray() as object;
                }
            });
        }

        /// <summary>
        /// Publish or not to publish call for item in cloud.
        /// </summary>
        /// <param name="name">Folder or file name.</param>
        /// <param name="fullPath">Full file or folder name.</param>
        /// <param name="publish">Publish or not to publish operation.</param>
        /// <param name="publishLink">Public item link if publish operation is used.</param>
        /// <returns>Public link or public item id.</returns>
        private async Task<string> PublishUnpulishLink(string name, string fullPath, bool publish, string publishLink)
        {
            var addFileRequest = Encoding.UTF8.GetBytes(
                string.Format(
                "{5}={0}&api={1}&token={2}&email={3}&x-email={4}",
                publish ? fullPath : publishLink.Replace(ConstSettings.PublishFileLink, string.Empty),
                2,
                this.Account.AuthToken,
                this.Account.LoginName,
                this.Account.LoginName,
                publish ? "home" : "weblink"));

            var url = new Uri(string.Format("{0}/api/v2/file/{1}", ConstSettings.CloudDomain, publish ? "publish" : "unpublish"));
            var request = (HttpWebRequest)WebRequest.Create(url.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "POST";
            request.ContentLength = addFileRequest.LongLength;
            request.Referer = string.Format("{0}/home{1}", ConstSettings.CloudDomain, fullPath.Substring(0, fullPath.LastIndexOf(name)));
            request.Headers.Add("Origin", ConstSettings.CloudDomain);
            request.Host = url.Host;
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "*/*";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetRequestStream, asyncResult => request.EndGetRequestStream(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var s = t.Result)
                {
                    s.Write(addFileRequest, 0, addFileRequest.Length);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var publicLink = (string)JsonParser.Parse(this.ReadResponseAsText(response), PObject.PublicLink);
                            if (publish)
                            {
                                return ConstSettings.PublishFileLink + publicLink;
                            }
                            else
                            {
                                return publicLink;
                            }
                        }

                        throw new HttpListenerException((int)response.StatusCode);
                    }
                }
            });
        }

        /// <summary>
        /// Rename item on server.
        /// </summary>
        /// <param name="name">File or folder name.</param>
        /// <param name="fullPath">Full path of the file or folder.</param>
        /// <param name="newName">New file or path name.</param>
        /// <returns>True or false result operation.</returns>
        private async Task<bool> Rename(string name, string fullPath, string newName)
        {
            var moveRequest = Encoding.UTF8.GetBytes(string.Format("home={0}&api={1}&token={2}&email={3}&x-email={3}&conflict=rename&name={4}", fullPath, 2, this.Account.AuthToken, this.Account.LoginName, newName));

            var url = new Uri(string.Format("{0}/api/v2/file/rename", ConstSettings.CloudDomain));
            var request = (HttpWebRequest)WebRequest.Create(url.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "POST";
            request.ContentLength = moveRequest.LongLength;
            request.Referer = string.Format("{0}/home{1}", ConstSettings.CloudDomain, fullPath.Substring(0, fullPath.LastIndexOf(name)));
            request.Headers.Add("Origin", ConstSettings.CloudDomain);
            request.Host = url.Host;
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "*/*";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetRequestStream, asyncResult => request.EndGetRequestStream(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var s = t.Result)
                {
                    s.Write(moveRequest, 0, moveRequest.Length);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception();
                        }

                        return true;
                    }
                }
            });
        }

        /// <summary>
        /// Move or copy item on server.
        /// </summary>
        /// <param name="sourceName">Source file or path name.</param>
        /// <param name="sourceFullPath">Full path source or file name.</param>
        /// <param name="destinationPath">Destination path to cope or move.</param>
        /// <param name="move">Move or copy operation.</param>
        /// <returns>True or false result operation.</returns>
        private async Task<bool> MoveOrCopy(string sourceName, string sourceFullPath, string destinationPath, bool move)
        {
            var moveRequest = Encoding.UTF8.GetBytes(string.Format("home={0}&api={1}&token={2}&email={3}&x-email={3}&conflict=rename&folder={4}", sourceFullPath, 2, this.Account.AuthToken, this.Account.LoginName, destinationPath));

            var url = new Uri(string.Format("{0}/api/v2/file/{1}", ConstSettings.CloudDomain, move ? "move" : "copy"));
            var request = (HttpWebRequest)WebRequest.Create(url.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "POST";
            request.ContentLength = moveRequest.LongLength;
            request.Referer = string.Format("{0}/home{1}", ConstSettings.CloudDomain, sourceFullPath.Substring(0, sourceFullPath.LastIndexOf(sourceName)));
            request.Headers.Add("Origin", ConstSettings.CloudDomain);
            request.Host = url.Host;
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "*/*";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetRequestStream, asyncResult => request.EndGetRequestStream(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var s = t.Result)
                {
                    s.Write(moveRequest, 0, moveRequest.Length);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception();
                        }

                        return true;
                    }
                }
            });
        }

        /// <summary>
        /// Remove file or folder.
        /// </summary>
        /// <param name="name">File or folder name.</param>
        /// <param name="fullPath">Full file or folder name.</param>
        /// <returns>True or false result operation.</returns>
        private async Task<bool> Remove(string name, string fullPath)
        {
            var removeRequest = Encoding.UTF8.GetBytes(string.Format("home={0}&api={1}&token={2}&email={3}&x-email={3}", fullPath, 2, this.Account.AuthToken, this.Account.LoginName));

            var url = new Uri(string.Format("{0}/api/v2/file/remove", ConstSettings.CloudDomain));
            var request = (HttpWebRequest)WebRequest.Create(url.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "POST";
            request.ContentLength = removeRequest.LongLength;
            request.Referer = string.Format("{0}/home{1}", ConstSettings.CloudDomain, fullPath.Substring(0, fullPath.LastIndexOf(name)));
            request.Headers.Add("Origin", ConstSettings.CloudDomain);
            request.Host = url.Host;
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "*/*";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetRequestStream, asyncResult => request.EndGetRequestStream(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var s = t.Result)
                {
                    s.Write(removeRequest, 0, removeRequest.Length);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception();
                        }

                        return true;
                    }
                }
            });
        }

        /// <summary>
        /// Create file record in the cloud.
        /// </summary>
        /// <param name="fileInfo">File info.</param>
        /// <returns>True or false result operation.</returns>
        private async Task<bool> AddFileInCloud(File fileInfo)
        {
            var hasFile = fileInfo.Hash != null && fileInfo.Size.DefaultValue != 0;
            var filePart = hasFile ? string.Format("&hash={0}&size={1}", fileInfo.Hash, fileInfo.Size.DefaultValue) : string.Empty;
            var addFileRequest = Encoding.UTF8.GetBytes(string.Format("home={0}&conflict=rename&api={1}&token={2}", fileInfo.FulPath, 2, this.Account.AuthToken) + filePart);

            var url = new Uri(string.Format("{0}/api/v2/{1}/add", ConstSettings.CloudDomain, hasFile ? "file" : "folder"));
            var request = (HttpWebRequest)WebRequest.Create(url.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = this.Account.Cookies;
            request.Method = "POST";
            request.ContentLength = addFileRequest.LongLength;
            request.Referer = string.Format("{0}/home{1}", ConstSettings.CloudDomain, HttpUtility.UrlEncode(fileInfo.FulPath.Substring(0, fileInfo.FulPath.LastIndexOf(fileInfo.Name))));
            request.Headers.Add("Origin", ConstSettings.CloudDomain);
            request.Host = url.Host;
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "*/*";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetRequestStream, asyncResult => request.EndGetRequestStream(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var s = t.Result)
                {
                    s.Write(addFileRequest, 0, addFileRequest.Length);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception();
                        }

                        return true;
                    }
                }
            });
        }

        /// <summary>
        /// Write file in the stream.
        /// </summary>
        /// <param name="fileInfo">File in file system.</param>
        /// <param name="outputStream">Stream to writing.</param>
        /// <param name="includeProgressEvent">On or off progress change event.</param>
        /// <param name="operation">Currently operation type.</param>
        private void WriteBytesInStream(FileInfo fileInfo, Stream outputStream, bool includeProgressEvent = false, OperationType operation = OperationType.None)
        {
            using (var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 8196))
            {
                using (var source = new BinaryReader(stream))
                {
                    this.WriteBytesInStream(source, outputStream, fileInfo.Length, includeProgressEvent, operation);
                }
            }
        }

        /// <summary>
        /// Write one stream to another.
        /// </summary>
        /// <param name="sourceStream">Source stream reader.</param>
        /// <param name="outputStream">Stream to writing.</param>
        /// <param name="length">Stream length.</param>
        /// <param name="includeProgressEvent">On or off progress change event.</param>
        /// <param name="operation">Currently operation type.</param>
        private void WriteBytesInStream(BinaryReader sourceStream, Stream outputStream, long length, bool includeProgressEvent = false, OperationType operation = OperationType.None)
        {
            if (includeProgressEvent)
            {
                this.OnChangedProgressPercent(new ProgressChangedEventArgs(
                                0,
                                new ProgressChangeTaskState()
                                {
                                    Type = operation,
                                    TotalBytes = new FileSize()
                                    {
                                        DefaultValue = length
                                    },
                                    BytesInProgress = new FileSize()
                                    {
                                        DefaultValue = 0L
                                    }
                                }));
            }

            int bufferLength = 8192;
            var totalWritten = 0L;
            if (length < bufferLength)
            {
                sourceStream.BaseStream.CopyTo(outputStream);
            }
            else
            {
                double percentComplete = 0;
                while (length > totalWritten)
                {
                    var bytes = sourceStream.ReadBytes(bufferLength);
                    outputStream.Write(bytes, 0, bufferLength);

                    totalWritten += bufferLength;
                    if (length - totalWritten < bufferLength)
                    {
                        bufferLength = (int)(length - totalWritten);
                    }

                    if (includeProgressEvent && length != 0 && length >= totalWritten)
                    {
                        double tempPercentComplete = 100.0 * (double)totalWritten / (double)length;
                        if (tempPercentComplete - percentComplete >= 1)
                        {
                            percentComplete = tempPercentComplete;
                            this.OnChangedProgressPercent(new ProgressChangedEventArgs(
                                (int)percentComplete,
                                new ProgressChangeTaskState()
                                {
                                    Type = operation,
                                    TotalBytes = new FileSize()
                                    {
                                        DefaultValue = length
                                    },
                                    BytesInProgress = new FileSize()
                                    {
                                        DefaultValue = totalWritten
                                    }
                                }));
                        }
                    }
                }
            }

            if (includeProgressEvent)
            {
                this.OnChangedProgressPercent(new ProgressChangedEventArgs(
                                100,
                                new ProgressChangeTaskState()
                                {
                                    Type = operation,
                                    TotalBytes = new FileSize()
                                    {
                                        DefaultValue = length
                                    },
                                    BytesInProgress = new FileSize()
                                    {
                                        DefaultValue = totalWritten == 0 ? length : totalWritten
                                    }
                                }));
            }
        }

        /// <summary>
        /// Write byte array in the stream.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <param name="outputStream">Stream to writing.</param>
        /// <param name="includeProgressEvent">On or off progress change event.</param>
        /// <param name="operation">Currently operation type.</param>
        private void WriteBytesInStream(byte[] bytes, Stream outputStream, bool includeProgressEvent = false, OperationType operation = OperationType.None)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var source = new BinaryReader(stream))
                {
                    this.WriteBytesInStream(source, outputStream, bytes.LongLength, includeProgressEvent, operation);
                }
            }
        }

        /// <summary>
        /// Get shard info that to do post get request.
        /// </summary>
        /// <param name="shardType">Shard type as numeric type.</param>
        /// <returns>Shard info.</returns>
        private async Task<ShardInfo> GetShardInfo(ShardType shardType)
        {
            CookieContainer cookie = null;
            return await this.GetShardInfo(shardType, false, cookie);
        }

        /// <summary>
        /// Get shard info that to do post get request. Can be use for anonymous user.
        /// </summary>
        /// <param name="shardType">Shard type as numeric type.</param>
        /// <param name="useAnonymousUser">To get anonymous user.</param>
        /// <param name="cookie">Generated cookie.</param>
        /// <returns>Shard info.</returns>
        private async Task<ShardInfo> GetShardInfo(ShardType shardType, bool useAnonymousUser, CookieContainer cookie)
        {
            this.CheckAuth();
            var uri = new Uri(string.Format("{0}/api/v2/dispatcher?{2}={1}", ConstSettings.CloudDomain, !useAnonymousUser ? this.Account.AuthToken : 2.ToString(), !useAnonymousUser ? "token" : "api"));
            var request = (HttpWebRequest)WebRequest.Create(uri.OriginalString);
            request.Proxy = this.Account.Proxy;
            request.CookieContainer = !useAnonymousUser ? Account.Cookies : new CookieContainer();
            request.Method = "GET";
            request.ContentType = ConstSettings.DefaultRequestType;
            request.Accept = "application/json";
            request.UserAgent = ConstSettings.UserAgent;
            var task = Task.Factory.FromAsync(request.BeginGetResponse, asyncResult => request.EndGetResponse(asyncResult), (object)null);
            return await task.ContinueWith((t) =>
            {
                using (var response = t.Result as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        cookie = request.CookieContainer;
                        return (ShardInfo)JsonParser.Parse(this.ReadResponseAsText(response), PObject.Shard, shardType.GetEnumDescription());
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            });
        }

        /// <summary>
        /// Need to add this function for all calls.
        /// </summary>
        private void CheckAuth()
        {
            if (this.Account == null)
            {
                throw new Exception("Account is null or empty");
            }

            if (string.IsNullOrEmpty(this.Account.AuthToken))
            {
                if (!this.Account.Login())
                {
                    throw new Exception("Auth token has't been retrieved.");
                }
            }
        }
    }
}
