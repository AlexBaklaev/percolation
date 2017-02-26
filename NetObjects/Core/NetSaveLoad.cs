using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace NetObjects.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class NetSaveLoad : INetCreator
    {
        /// <summary>
        /// 
        /// </summary>
        private string _path; 
        /// <summary>
        /// 
        /// </summary>
        public NetSaveLoad()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public NetSaveLoad(string path)
        {
            _path = path;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="net"></param>
        /// <param name="path"></param>
        public bool NetSave(Node[] net, string path)
        {
            try
            {
                var streamwriter = new StreamWriter(path + ".xml",false);
                var x = new XmlSerializer(net.GetType());
                x.Serialize(streamwriter, net);
                streamwriter.Flush();
                streamwriter.Dispose();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node[] CreateNet()
        {
            try
            {
                var deserializer = new XmlSerializer(typeof(Node[]));
                TextReader textReader = new StreamReader(_path + ".xml");
                var ret = (Node[])deserializer.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
                return ret;
            }
            catch (Exception )
            {
                return null;
            }
        }

    }
}
