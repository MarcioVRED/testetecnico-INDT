using Insurance.Application.Contracts;
using Insurance.PropostaService.Api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.PropostaService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostaController : ControllerBase
    {
        private readonly IPropostaApplicationService _service;

        public PropostaController(IPropostaApplicationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria uma nova proposta de seguro.
        /// </summary>
        /// <param name="dto">Dados da proposta</param>
        /// <response code="200">Proposta criada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(PropostaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] CreatePropostaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var proposta = await _service.CriarProposta(dto.Cliente, dto.Valor);
            return Ok(PropostaDto.FromDomain(proposta));
        }

        /// <summary>
        /// Lista todas as propostas.
        /// </summary>
        /// <response code="200">Lista de propostas retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PropostaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            var propostas = await _service.ListarPropostas();
            var dtos = propostas.Select(PropostaDto.FromDomain);
            return Ok(dtos);
        }

        /// <summary>
        /// Altera o status de uma proposta existente.
        /// </summary>
        /// <param name="id">ID da proposta</param>
        /// <param name="status">Novo status</param>
        /// <response code="200">Status alterado com sucesso</response>
        /// <response code="400">Status inválido</response>
        /// <response code="404">Proposta não encontrada</response>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarStatus(Guid id, [FromBody] UpdateStatusPropostaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AlterarStatus(id, dto.Status);
            return result ? Ok() : NotFound($"Proposta com id {id} não encontrada.");
        }
    }
}
