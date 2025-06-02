using MauiAppTempoSQLite.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoSQLite.Services
{
    /// <summary>
    /// Serviço responsável por buscar dados meteorológicos da API OpenWeatherMap
    /// Implementa o padrão Service Layer para separar a lógica de acesso a dados externos
    /// </summary>
    public class DataService
    {
        /// <summary>
        /// Método estático assíncrono que busca previsão do tempo para uma cidade específica
        /// </summary>
        /// <param name="cidade">Nome da cidade para consulta meteorológica</param>
        /// <returns>Objeto Tempo com dados meteorológicos ou null se houver erro</returns>
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            // Inicializa variável que armazenará o resultado
            // Declarada como nullable para retornar null em caso de erro
            Tempo? t = null;

            // Chave da API OpenWeatherMap (ATENÇÃO: deveria estar em configuração segura)
            // Esta chave permite acesso aos serviços da API meteorológica
            string chave = "6135072afe7f6cec1537d5cb08a5a1a2";

            // Constrói a URL da API com parâmetros:
            // - q: nome da cidade a ser consultada
            // - units=metric: temperaturas em Celsius
            // - appid: chave de autenticação da API
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            // Using garante que o HttpClient seja descartado corretamente
            // Evita vazamentos de recursos de rede
            using (HttpClient client = new HttpClient())
            {
                // Faz requisição HTTP GET assíncrona para a API
                // await suspende a execução até a resposta chegar
                HttpResponseMessage resp = await client.GetAsync(url);

                // Verifica se a requisição foi bem-sucedida (status 200-299)
                if (resp.IsSuccessStatusCode)
                {
                    // Lê o conteúdo da resposta como string JSON
                    string json = await resp.Content.ReadAsStringAsync();

                    // Usa Newtonsoft.Json para fazer parse do JSON em objeto dinâmico
                    // JObject permite navegar pela estrutura JSON usando indexadores
                    var rascunho = JObject.Parse(json);

                    // Cria DateTime base (01/01/0001 00:00:00)
                    // Será usado para converter timestamps Unix em DateTime
                    DateTime time = new();

                    // Converte timestamp Unix do nascer do sol para DateTime local
                    // rascunho["sys"]["sunrise"] contém segundos desde 1970 (Unix timestamp)
                    // AddSeconds adiciona os segundos ao DateTime base
                    // ToLocalTime() converte para horário local do dispositivo
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();

                    // Converte timestamp Unix do pôr do sol para DateTime local
                    // Mesmo processo do sunrise
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    // Cria novo objeto Tempo com dados extraídos do JSON
                    // Usa object initializer syntax para definir propriedades
                    t = new()
                    {
                        // Coordenadas geográficas da cidade
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],

                        // Informações meteorológicas
                        // [0] pega o primeiro elemento do array weather
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],

                        // Temperaturas mínima e máxima em Celsius
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],

                        // Velocidade do vento (m/s quando units=metric)
                        speed = (double)rascunho["wind"]["speed"],

                        // Visibilidade em metros
                        visibility = (int)rascunho["visibility"],

                        // Horários convertidos para string
                        // ToString() usa formato padrão do sistema
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; // Fecha object initializer do Tempo

                } // Fecha if - verifica se status HTTP foi sucesso

            } // Fecha using - garante dispose do HttpClient

            // Retorna objeto Tempo preenchido ou null se houve erro
            return t;
        }
    }
}