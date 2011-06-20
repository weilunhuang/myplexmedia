using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;


namespace PlexMediaCenter.Util {

    public class PlexAuthenticationProxy {

        private HttpListener listener = null;
      
        public PlexAuthenticationProxy() {
           
        }

        /// <summary>
        /// The port number the proxy will run on (default: 46469)
        /// </summary>
        public int Port {
            get { return _port; }
            set { _port = value; }
        } private int _port = 0;

        /// <summary>
        /// Number of bytes in the buffer (default: 1024)
        /// </summary>
        public int Buffer {
            get { return _buffer; }
            set { _buffer = value; }
        } private int _buffer = 1024;

       
        /// <summary>
        /// Gets a value indicating the proxy is running
        /// </summary>
        public bool Started {
            get {
                return (listener != null && listener.IsListening);
            }
        }

        /// <summary>
        /// Starts the proxy
        /// </summary>
        public void Start() {
            if (this.listener != null)
                this.listener.Close();

            this.listener = new HttpListener();

            // if port is 0 find one to use.
            if (_port == 0)
                _port = getAvailablePort();

            string prefix = String.Format("http://localhost:{0}/", _port);
            try {
                this.listener.Prefixes.Add(prefix);
                this.listener.Start();
                
                this.listener.BeginGetContext(new AsyncCallback(listenerCallback), this.listener);
                //Log.Info("[MyTrailers] VideoProxy started listening on port: {0}", _port);
            } catch (Exception e) {
                //Log.Error("[MyTrailers] VideoProxy could not be started: {0}", e.Message);
            }
        }


        /// <summary>
        /// Stops the proxy
        /// </summary>
        public void Stop() {
            if (listener != null && listener.IsListening) {
                listener.Close();
                //Log.Info("[MyTrailers] VideoProxy stopped listening.");
            }
        }

        /// <summary>
        /// Returns the proxy url from the actual url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetProxyUrl(string url) {
            return ("http://localhost:" + _port.ToString() + "/" + url.Remove(0, 7));
        }

        /// <summary>
        /// Triggered when a client connects to the proxy
        /// </summary>
        /// <param name="result"></param>
        private void listenerCallback(IAsyncResult result) {
            HttpListener listener = (HttpListener)result.AsyncState;
            if (listener != null && listener.IsListening) {
                //Log.Info("[MyTrailers] VideoProxy Client Connected.");
                // Listen Again
                listener.BeginGetContext(new AsyncCallback(listenerCallback), listener);
                // Process Request
                processRequest(listener.EndGetContext(result));
            }
        }

        /// <summary>
        /// Handles a new client request
        /// </summary>
        /// <param name="context"></param>
        private void processRequest(HttpListenerContext context) {
            if (context == null)
                return;

            HttpListenerRequest request = context.Request;

            // Create the actual url from the request
            string[] urlArray = request.Url.AbsoluteUri.Remove(0, 7).Split('/');
            string actualUrl = "http://" + urlArray[1] + @"/";
            for (int i = 2; i < urlArray.Length; i++)
                actualUrl += urlArray[i] + @"/";
            actualUrl = actualUrl.Remove(actualUrl.Length - 1, 1);

            //Log.Info("[MyTrailers] VideoProxy Processing Request: {0}", actualUrl);

            HttpWebRequest proxyRequest = (HttpWebRequest)WebRequest.Create(actualUrl);
            WebClient client = new WebClient();
            Plex.PlexInterface.PlexServerCurrent.AddAuthHeaders(ref client);
            proxyRequest.Headers.Add(client.Headers);
            // Copy Request Headers
            foreach (string key in request.Headers.AllKeys) {
                try {
                    proxyRequest.Headers.Add(key, request.Headers[key]);
                } catch (Exception) { }
            }          

            HttpWebResponse proxyResponse = null;
            HttpListenerResponse response = context.Response;
            try {
                proxyResponse = (HttpWebResponse)proxyRequest.GetResponse();
                //Log.Info("[MyTrailers] VideoProxy Response: StatusCode={0}, ContentType={1}, ContentLength={2}", proxyResponse.StatusCode.ToString(), proxyResponse.ContentType, proxyResponse.ContentLength);

                // Copy Headers
                foreach (string key in proxyResponse.Headers.AllKeys) {
                    try { response.AddHeader(key, proxyResponse.GetResponseHeader(key)); } catch (Exception) { }
                }

                // Set protected headers
                response.StatusCode = (int)proxyResponse.StatusCode;
                response.ContentType = proxyResponse.ContentType;
                response.ContentLength64 = proxyResponse.ContentLength;
                response.ProtocolVersion = proxyResponse.ProtocolVersion;
                response.KeepAlive = true;
            } catch (WebException e) {
                //Log.Debug("[MyTrailers] VideoProxy didn't receive a valid response from the requested resource: ", e.Message);
            }

            if (!proxyRequest.HaveResponse) {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
            } else {
                Stream inputStream = proxyResponse.GetResponseStream();
                Stream outputStream = context.Response.OutputStream;

                byte[] buffer = new byte[_buffer];
                while (true) {
                    int read = inputStream.Read(buffer, 0, buffer.Length);
                    // if we reached the end of the stream, break the loop
                    if (read <= 0)
                        break;

                    try {
                        outputStream.Write(buffer, 0, read);
                    } catch (HttpListenerException e) {
                        if ((e.ErrorCode == 1229) || (e.ErrorCode == 64)) {
                            //Log.Info("[MyTrailers] VideoProxy: Lost Client Connection.");
                            break;
                        } else {
                            //Log.Info("[MyTrailers] VideoProxy Error: ", e.Message);
                            break;
                        }
                    } catch (Exception e) {
                        //Log.Info("[MyTrailers] VideoProxy Error: ", e.Message);
                    }
                }

                inputStream.Close();
                proxyResponse.Close();
                try {
                    outputStream.Flush();
                    outputStream.Close();
                    response.Close();
                } catch (Exception) { }
            }
            //Log.Info("[MyTrailers] VideoProxy: Client Disconnected. ({0})", actualUrl);
        }

        public static int getAvailablePort() {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= 46000
                               select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= 46000
                               select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= 46000
                               select n.Port);

            portArray.Sort();

            for (int i = 46000; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }

    }
}