using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
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
            Memory<byte> buffer = new Memory<byte>();
            await _stream.ReadAsync(buffer);
            this.MessageRecieved?.Invoke(this, JsonConvert.DeserializeObject<Message>(UTF8Encoding.Default.GetString(buffer.ToArray())));            
            this.ReadMessage();
        }

        public async Task WriteMessage(Message msg)
        {
            await _stream.WriteAsync(UTF8Encoding.Default.GetBytes(JsonConvert.SerializeObject(msg)));
        }
    }
}
