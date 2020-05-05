using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace httpServCsharp
{
    public class MyListener
    {
        public static void startListener(List<string> prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Pas supporté !");
                return;
            }

            // Creation du listener
            HttpListener listener = new HttpListener();
            // Ajouts des uri
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();

                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                Console.WriteLine($"On a reçu une requête de : {request.Url}");
                Console.WriteLine(documentContents);

                // Obtain a response object.
                HttpListenerResponse response = context.Response;
              
                    try
                    {
                        var handled = false;
                        switch (context.Request.Url.AbsolutePath)
                        {
                            //This is where we do different things depending on the URL
                            //TODO: Add cases for each URL we want to respond to
                            case "/login":
                                switch (context.Request.HttpMethod)
                                {
                                    case "GET":
                                        //Get the current settings
                                        response.ContentType = "application/json";

                                        //This is what we want to send back
                                        //var responseBody = JsonConvert.SerializeObject(MyApplicationSettings);
                                        string responseStringGET = " <div class='container'>< label for= 'uname'>< b > Username </ b ></ label >< input type = 'text' placeholder = 'Enter Username' name = 'uname' required >< label for= 'psw' >< b > Password </ b ></ label >< input type = 'password' placeholder = 'Enter Password' name = 'psw' required >< button type = 'submit' > Login </ button ></ div >";
                                        //Write it to the response stream
                                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseStringGET);
                                        System.IO.Stream output2 = response.OutputStream;
                                        output2.Write(buffer, 0, buffer.Length);
                                        // You must close the output stream.
                                        output2.Close();
                                        break;

                                   // case "POST":
                                       
                                        //using (var body = context.Request.InputStream)
                                        //using (var reader = new StreamReader(body, context.Request.ContentEncoding))
                                        //{
                                            //Get the data that was sent to us
                                          //  var json = reader.ReadToEnd();
         
                                            //Return 204 No Content to say we did it successfully
                                            //response.StatusCode = 204;
                                            //handled = true;
                                        //}
                                        //break;
                                }
                                break;
                        default:
                            string responseString = "<HTML><BODY> Bienvenue sur la page d'accueil !</BODY></HTML>";
                            byte[] bufferDef = System.Text.Encoding.UTF8.GetBytes(responseString);
                            // Get a response stream and write the response to it.
                            response.ContentLength64 = bufferDef.Length;
                            System.IO.Stream output = response.OutputStream;
                            output.Write(bufferDef, 0, bufferDef.Length);
                            // You must close the output stream.
                            output.Close();
                            break;
                    }
                        if (!handled)
                        {
                            response.StatusCode = 404;
                        }
                    }
                    catch (Exception e)
                    {
                    //Return the exception details the client - you may or may not want to do this
                        string responseString = "<HTML><BODY>Error !</BODY></HTML>";
                        response.StatusCode = 500;
                        response.ContentType = "application/json";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);

                    }
            }
            listener.Stop();
        }
    }
}
