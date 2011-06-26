using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlexMediaCenter.Plex;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading;
using PlexMediaCenter.Plex.Connection;
using PlexMediaCenter.Plex.Data.Types;
using System.Media;

namespace PlexMediaCenter.Util {
    public static class Transcoding {

       // private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static bool IsBuffering { get; set; }

        public static event OnBufferingProgressEventHandler OnBufferingProgress;
        public delegate void OnBufferingProgressEventHandler(int currentProgress);

        public static event OnPlayBufferedMediaEventHandler OnPlayBufferedMedia;
        public delegate void OnPlayBufferedMediaEventHandler(string localBufferPath);
        
        private const string _plexApiPublicKey = "KQMIY6GATPC63AIMC4R2";
        private const string _plexApiSharedSecret = "k3U6GLkZOoNIoSgjDshPErvqMIFdE0xMTx8kgsrhnC0=";

        private static BackgroundWorker _mediaBufferer;
        private static WebClient _mediaFetcher;
        private static int Quality { get; set; }
        private static int Buffer { get; set; }
        private const string _bufferFile = @"D:\buffer.avi";
        private const int _defaultBuffer = 3;
        private const int _defaultQuality = 5;

        static Transcoding() {
            //logger.Info(" started...");
            DeleteBufferFile();
            Buffer = _defaultBuffer;
            Quality = _defaultQuality;
            _mediaFetcher = new WebClient();
            _mediaBufferer = new BackgroundWorker();
            _mediaBufferer.WorkerSupportsCancellation = true;
            _mediaBufferer.WorkerReportsProgress = true;
            _mediaBufferer.ProgressChanged += new ProgressChangedEventHandler(MediaBufferer_ProgressChanged);
            _mediaBufferer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_mediaBufferer_RunWorkerCompleted);
            _mediaBufferer.DoWork += new DoWorkEventHandler(MediaBufferer_DoWork);

           

        }

        private static void CreateDummyFile(string fileName, long length) {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)) {
                fileStream.SetLength(length);
            }
        }

        private static void DeleteBufferFile() {
            try {
                if (File.Exists(_bufferFile)) {
                    File.Delete(_bufferFile);
                }

            } catch (Exception e) {
                //logger.FatalException("Unable reset local media buffer!", e);
            }

        }       

        static void MediaBufferer_DoWork(object sender, DoWorkEventArgs e) {
            //logger.Info("BackGroundWorker - Buffering...");
            if (e.Argument is IEnumerable<string>) {
                IsBuffering = true;
                using (FileStream _bufferedMedia = new FileStream(_bufferFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                    //_bufferedMedia.SetLength(366485736);
                    _bufferedMedia.Seek(366485736, SeekOrigin.Begin);
                    _bufferedMedia.WriteByte(0);
                    _bufferedMedia.Seek(0, SeekOrigin.Begin);                    
                    int bufferedSegments = 0;
                    foreach (string segment in (IEnumerable<string>)e.Argument) {
                        if (_mediaBufferer.CancellationPending) {
                            //logger.Info("BackGroundWorker - CancellationPending detected - cancelling asynchronous buffering...");
                            e.Cancel = true;
                            _bufferedMedia.Close();
                            return;
                        }
                        if (bufferedSegments <= Buffer) {
                            _mediaBufferer.ReportProgress(bufferedSegments * 100 / Buffer);
                        }
                        byte[] data = _mediaFetcher.DownloadData(segment);

                        _bufferedMedia.Write(data, 0, data.Length);
                        _bufferedMedia.Flush();

                        SystemSounds.Beep.Play();
                        if (++bufferedSegments == Buffer) {
                            OnPlayBufferedMedia(_bufferFile);
                        }

                    }
                }
            }
        }

        static void MediaBufferer_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            Console.WriteLine(e.ProgressPercentage);
            OnBufferingProgress(e.ProgressPercentage);
        }

        static void _mediaBufferer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled) {
                //logger.Warn("BackGroundWorker -Buffering completed and was cancelled");
            } else if (e.Error != null) {
                //logger.ErrorException("BackGroundWorker -Buffering completed with Exception", e.Error);
            } else {
                //logger.Info("BackGroundWorker -Buffering completed and was successful");
            }
            IsBuffering = false;
            DeleteBufferFile();
        }

        private static void BufferMediaAsync(IEnumerable<string> segmentedParts) {
            StopBuffering();
            _mediaBufferer.RunWorkerAsync(segmentedParts);
        }

        public static void StopBuffering() {
            if (_mediaBufferer.IsBusy) {
                //logger.Info("Request Buffering Cancellation");
                _mediaBufferer.CancelAsync();
            }
        }



        public static void PlayBackMedia(MediaContainerVideo video) {
            BufferMedia(video.Media[0].Part[0].key);
        }

        internal static void BufferMedia(string partKey, int offset = 0) {
            BufferMediaAsync(GetM3U8PlaylistItems(PlexInterface.PlexServerCurrent, partKey));
        }

        public static IEnumerable<string> GetM3U8PlaylistItems(PlexServer plexServer, string partKey) {
            string response = _mediaFetcher.DownloadString(GetM3U8PlaylistUrl(plexServer, partKey));
            string session = response.Substring(response.IndexOf("session")).Replace("\n", "");

            string playListRequest = plexServer.UriPlexBase.AbsoluteUri + "video/:/transcode/segmented/" + session;

            string cookie = _mediaFetcher.ResponseHeaders[HttpResponseHeader.SetCookie];
            if (cookie != null && cookie.Length > 0)
                _mediaFetcher.Headers[HttpRequestHeader.Cookie] = cookie;
            string playList = _mediaFetcher.DownloadString(playListRequest);
            List<string> playListItems = playList.Split(new char[] { '\n' }).Where(item => item.EndsWith(".ts")).ToList();
            foreach (string currentItem in playListItems) {
                yield return playListRequest.Replace("index.m3u8", currentItem);
            }
        }

        public static Uri GetM3U8PlaylistUrl(PlexServer plexServer, string partKey, long offset = 0, int quality = _defaultQuality, bool is3G = true) {
            string transcodePath = "/video/:/transcode/segmented/start.m3u8?";
            transcodePath += "identifier=com.plexapp.plugins.library";
            transcodePath += "&offset=" + offset;
            transcodePath += "&qualitiy=" + quality;
            //transcodePath += "&minquality=0";
            //transcodePath += "&maxquality=1";
            transcodePath += "&3g=" + (is3G ? "1" : "0");
            transcodePath += "&url=" + Uri.EscapeDataString("http://localhost:32400" + partKey);
            transcodePath += GetPlexAuthParameters(plexServer, transcodePath);
            transcodePath += PlexCapabilitiesClient.GetClientCapabilities();
            transcodePath += "&httpCookies=";
            transcodePath += "&userAgent=";

            return new Uri(plexServer.UriPlexBase + transcodePath.Remove(0, 1));
        }
        
        public static Uri GetFlvStreamUrl(PlexServer plexServer, string partKey, long offset = 0, int quality = _defaultQuality, bool is3G = true) {
            //Request: GET /video/:/transcode/generic.flv?format=flv&videoCodec=libx264&vpre=video-embedded-h264&videoBitrate=5000&audioCodec=libfaac&apre=audio-embedded-aac&audioBitrate=128&size=640x480&fakeContentLength=2000000000&url=http%3A%2F%2F192%2E168%2E1%2E87%3A32400%2Fvideo%2F
            string transcodePath = "/video/:/transcode/generic.flv?";
            transcodePath += "format=flv";
            transcodePath += "&videoCodec=libx264&vpre=video-embedded-h264&videoBitrate=5000&audioCodec=libfaac&apre=audio-embedded-aac&audioBitrate=128&size=640x480&fakeContentLength=2000000000";
            transcodePath += "&url=" + Uri.EscapeDataString("http://localhost:32400" + partKey);
            transcodePath += GetPlexAuthParameters(plexServer, transcodePath);
            transcodePath += PlexCapabilitiesClient.GetClientCapabilities();
            //transcodePath += "&httpCookies=";
            //transcodePath += "&userAgent=";

            return new Uri(plexServer.UriPlexBase + transcodePath.Remove(0, 1));
        }

        public static string GetPlexAuthParameters(PlexServer plexServer, string url) {
            string time = GetUnixTime();
            string authParameters = string.Empty;
            authParameters += "&X-Plex-User=" + plexServer.UserName;
            authParameters += "&X-Plex-Pass=" + plexServer.UserPass;
            authParameters += "&X-Plex-Access-Key=" + _plexApiPublicKey;
            authParameters += "&X-Plex-Access-Time=" + time;
            authParameters += "&X-Plex-Access-Code=" + Uri.EscapeDataString(GetPlexApiToken(url, time));
            return authParameters;
        }

        public static string GetPlexUserPass(PlexServer plexServer) {           
            string authParameters = "?X-Plex-User=" + plexServer.UserName;
            authParameters += "&X-Plex-Pass=" + plexServer.UserPass;            
            return authParameters;
        }

        private static string GetPlexApiToken(string url, string time) {
            // the message to hash is url + an @ + the rounded time   
            string msg = url + "@" + time;
            byte[] privateKey = Convert.FromBase64String(_plexApiSharedSecret);

            // initialize a new HMACSHA256 class with the private key from Elan
            HMACSHA256 hmac = new HMACSHA256(privateKey);

            // compute the hash of the message. Note: .net is unicode double byte, so when we get the bytes
            // from the message we have to be sure to use UTF8 decoders.
            hmac.ComputeHash(UTF8Encoding.UTF8.GetBytes(msg));

            //our new super secret token is our new hash converted to a Base64 string
            return Convert.ToBase64String(hmac.Hash);
        }

        private static string GetUnixTime() {
            // unix time is the number of milliseconds from 1/1/1970 to now..          
            DateTime jan1 = new DateTime(1970, 1, 1, 0, 0, 0);
            double dTime = (DateTime.Now - jan1).TotalMilliseconds;
            // as per the Javascript example, round up the Unix time
            string time = Math.Round(dTime / 1000).ToString();
            // the basic url WITH the part key is:
            return time;
        }
    }

    public static class StreamExtensions {
        private const int DEFAULT_BUFFER_SIZE = short.MaxValue; // +32767
        public static void CopyTo(this Stream input, Stream output) {
            input.CopyTo(output, DEFAULT_BUFFER_SIZE);
            return;
        }
        public static void CopyTo(this Stream input, Stream output, int bufferSize) {
            if (!input.CanRead) throw new InvalidOperationException("input must be open for reading");
            if (!output.CanWrite) throw new InvalidOperationException("output must be open for writing");

            byte[][] buf = { new byte[bufferSize], new byte[bufferSize] };
            int[] bufl = { 0, 0 };
            int bufno = 0;
            IAsyncResult read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);
            IAsyncResult write = null;

            while (true) {

                // wait for the read operation to complete
                read.AsyncWaitHandle.WaitOne();
                bufl[bufno] = input.EndRead(read);

                // if zero bytes read, the copy is complete
                if (bufl[bufno] == 0) {
                    break;
                }

                // wait for the in-flight write operation, if one exists, to complete
                // the only time one won't exist is after the very first read operation completes
                if (write != null) {
                    write.AsyncWaitHandle.WaitOne();
                    output.EndWrite(write);
                }

                // start the new write operation
                write = output.BeginWrite(buf[bufno], 0, bufl[bufno], null, null);

                // toggle the current, in-use buffer
                // and start the read operation on the new buffer
                bufno = (bufno == 0 ? 1 : 0);
                read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);

            }

            // wait for the final in-flight write operation, if one exists, to complete
            // the only time one won't exist is if the input stream is empty.
            if (write != null) {
                write.AsyncWaitHandle.WaitOne();
                output.EndWrite(write);
            }

            output.Flush();

            // return to the caller ;
            return;
        }
    }
}
