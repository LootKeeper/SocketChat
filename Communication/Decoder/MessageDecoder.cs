using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Communication.Model;
using Newtonsoft.Json;

namespace Communication.Decoder
{
    public class MessageDecoder
    {
        private NetworkStream _stream;
        public event EventHandler<Message> MessageRecieved;

        public MessageDecoder(NetworkStream stream)
        {
            this._stream = stream;
            this.ReadMessage();
        }

        public async Task ReadMessage()
        {
            try
            {
                byte[] buffer = new byte[1024];
                await _stream.ReadAsync(buffer, 0, buffer.Length);
                this.MessageRecieved?.Invoke(this, JsonConvert.DeserializeObject<Message>(UTF8Encoding.Default.GetString(buffer)));
                this.ReadMessage();
            }
            catch (Exception ex)
            {
                _stream.Close();
            }
        }

        public async Task WriteMessage(Message msg)
        {
            try
            {
                await _stream.WriteAsync(UTF8Encoding.Default.GetBytes(JsonConvert.SerializeObject(msg)));
            }
            catch (Exception ex)
            {
                _stream.Close();
            }
        }
    }
}
