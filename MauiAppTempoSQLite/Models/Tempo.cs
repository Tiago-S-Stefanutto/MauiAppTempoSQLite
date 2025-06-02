using SQLite;

namespace MauiAppTempoSQLite.Models
{
    /// <summary>
    /// Modelo de dados que representa informações meteorológicas de uma cidade
    /// Esta classe mapeia tanto para o banco de dados local SQLite quanto para dados da API de clima
    /// </summary>
    public class Tempo
    {
        /// <summary>
        /// Identificador único do registro no banco de dados
        /// [PrimaryKey] - Define como chave primária da tabela
        /// [AutoIncrement] - Valor será incrementado automaticamente pelo SQLite
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Nome da cidade para qual foram consultadas as informações meteorológicas
        /// Campo obrigatório (não nullable) usado para identificar a localização
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Data e hora em que a consulta meteorológica foi realizada
        /// Usado para histórico e controle de quando os dados foram obtidos
        /// </summary>
        public DateTime DataConsulta { get; set; }

        /// <summary>
        /// Longitude da coordenada geográfica da cidade
        /// Nullable (double?) - pode ser nulo se a informação não estiver disponível
        /// Usado para localização precisa no mapa
        /// </summary>
        public double? lon { get; set; }

        /// <summary>
        /// Latitude da coordenada geográfica da cidade
        /// Nullable (double?) - pode ser nulo se a informação não estiver disponível
        /// Usado para localização precisa no mapa
        /// </summary>
        public double? lat { get; set; }

        /// <summary>
        /// Temperatura mínima registrada (provavelmente em Celsius)
        /// Nullable (double?) - pode não estar disponível em todas as consultas
        /// Representa a menor temperatura esperada para o período
        /// </summary>
        public double? temp_min { get; set; }

        /// <summary>
        /// Temperatura máxima registrada (provavelmente em Celsius)
        /// Nullable (double?) - pode não estar disponível em todas as consultas
        /// Representa a maior temperatura esperada para o período
        /// </summary>
        public double? temp_max { get; set; }

        /// <summary>
        /// Visibilidade atmosférica (provavelmente em metros ou quilômetros)
        /// Nullable (int?) - indica o quão longe é possível enxergar
        /// Afetada por neblina, chuva, poluição, etc.
        /// </summary>
        public int? visibility { get; set; }

        /// <summary>
        /// Velocidade do vento (provavelmente em km/h ou m/s)
        /// Nullable (double?) - informação sobre intensidade do vento
        /// Importante para sensação térmica e condições gerais
        /// </summary>
        public double? speed { get; set; }

        /// <summary>
        /// Condição meteorológica principal (ex: "Clear", "Rain", "Clouds")
        /// Nullable (string?) - resumo geral do clima
        /// Geralmente uma palavra-chave da API meteorológica
        /// </summary>
        public string? main { get; set; }

        /// <summary>
        /// Descrição detalhada das condições meteorológicas
        /// Nullable (string?) - explicação mais completa do clima
        /// Ex: "céu limpo", "chuva fraca", "nuvens dispersas"
        /// </summary>
        public string? description { get; set; }

        /// <summary>
        /// Horário do nascer do sol (provavelmente em formato de string)
        /// Nullable (string?) - informação astronômica importante
        /// Pode estar em formato de hora local ou timestamp
        /// </summary>
        public string? sunrise { get; set; }

        /// <summary>
        /// Horário do pôr do sol (provavelmente em formato de string)
        /// Nullable (string?) - informação astronômica importante
        /// Pode estar em formato de hora local ou timestamp
        /// </summary>
        public string? sunset { get; set; }
    }
}