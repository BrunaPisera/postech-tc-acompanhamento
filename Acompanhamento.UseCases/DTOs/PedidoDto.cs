using Newtonsoft.Json;

namespace Acompanhamento.UseCases.DTOs
{
    [JsonObject]
    public class PedidoDto
    {
        [JsonProperty("IdPedido")]
        public Guid IdPedido { get; set; }

        [JsonProperty("ClienteName")]
        public string ClienteName { get; set; }
    }
}
