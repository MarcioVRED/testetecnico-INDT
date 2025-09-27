using Insurance.Application.Contracts;
using Insurance.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.ContratacaoService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratacaoController : ControllerBase
    {
        private readonly IContratacaoApplicationService _contratacaoService;

        public ContratacaoController(IContratacaoApplicationService contratacaoService)
        {
            _contratacaoService = contratacaoService;
        }

        /// <summary>
        /// Contrata uma proposta de seguro (somente se aprovada).
        /// </summary>
        /// <param name="propostaId">ID da proposta a ser contratada</param>
        /// <response code="200">Proposta contratada com sucesso</response>
        /// <response code="400">Proposta não encontrada ou não aprovada</response>
        [HttpPost("contratar/{propostaId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Contratar(Guid propostaId)
        {
            await _contratacaoService.ContratarProposta(propostaId);
            return Ok("Proposta contratada com sucesso!");
        }

        /// <summary>
        /// Lista todas as contratações.
        /// </summary>
        /// <response code="200">Lista retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContratacaoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var contratacoes = await _contratacaoService.ListarContratacoes();
            var dtos = contratacoes.Select(ContratacaoDto.FromDomain);
            return Ok(dtos);
        }

        /// <summary>
        /// Retorna uma contratação pelo ID.
        /// </summary>
        /// <param name="id">ID da contratação</param>
        /// <response code="200">Contratação encontrada</response>
        /// <response code="404">Contratação não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContratacaoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contratacao = await _contratacaoService.ObterPorId(id);
            return Ok(ContratacaoDto.FromDomain(contratacao));
        }
    }
}
