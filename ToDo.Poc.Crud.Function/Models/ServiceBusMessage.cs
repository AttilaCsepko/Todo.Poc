using System.Collections.Generic;

namespace ToDo.Poc.Crud.Function.Models
{
    public class ServiceBusMessage
    {
        public string Value { get; set; }
        public List<object> Formatters { get; set; }
        public List<object> ContentTypes { get; set; }
        public int StatusCode { get; set; }
    }
}
