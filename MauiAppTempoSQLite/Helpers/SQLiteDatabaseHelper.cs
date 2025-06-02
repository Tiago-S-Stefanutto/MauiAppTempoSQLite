using MauiAppTempoSQLite.Models;
using SQLite;

namespace MauiAppTempoSQLite.Helpers
{
    /// <summary>
    /// Classe helper para gerenciar operações de banco de dados SQLite
    /// Fornece métodos para inserir, deletar, buscar e listar registros da tabela Tempo
    /// </summary>
    public class SQLiteDatabaseHelper
    {
        // Campo readonly que mantém a conexão assíncrona com o banco SQLite
        // Readonly significa que só pode ser atribuído no construtor
        readonly SQLiteAsyncConnection _conn;

        /// <summary>
        /// Construtor da classe - inicializa a conexão com o banco de dados
        /// </summary>
        /// <param name="path">Caminho do arquivo do banco de dados SQLite</param>
        public SQLiteDatabaseHelper(string path)
        {
            // Cria uma nova conexão assíncrona com o banco SQLite usando o caminho fornecido
            _conn = new SQLiteAsyncConnection(path);

            // Cria a tabela 'Tempo' no banco se ela não existir
            // .Wait() força a operação assíncrona a ser executada de forma síncrona
            // Isso garante que a tabela seja criada antes de qualquer operação
            _conn.CreateTableAsync<Tempo>().Wait();
        }

        /// <summary>
        /// Insere um novo registro de Tempo no banco de dados
        /// </summary>
        /// <param name="p">Objeto Tempo a ser inserido</param>
        /// <returns>Task<int> representando o número de linhas afetadas</returns>
        public Task<int> Insert(Tempo p)
        {
            // Executa uma operação de inserção assíncrona
            // Retorna quantas linhas foram afetadas (normalmente 1 para inserção bem-sucedida)
            return _conn.InsertAsync(p);
        }

        /// <summary>
        /// Deleta um registro específico do banco de dados baseado no ID
        /// </summary>
        /// <param name="id">ID do registro a ser deletado</param>
        /// <returns>Task<int> representando o número de linhas deletadas</returns>
        public Task<int> Delete(int id)
        {
            // Acessa a tabela Tempo e deleta o registro onde o Id corresponde ao parâmetro
            // A expressão lambda (i => i.Id == id) define a condição de busca
            return _conn.Table<Tempo>().DeleteAsync(i => i.Id == id);
        }

        /// <summary>
        /// Busca todos os registros da tabela Tempo
        /// </summary>
        /// <returns>Task<List<Tempo>> com todos os registros ordenados por ID decrescente</returns>
        public Task<List<Tempo>> GetAll()
        {
            // Acessa a tabela Tempo, ordena por Id em ordem decrescente (mais recente primeiro)
            // e converte o resultado para uma lista assíncrona
            return _conn.Table<Tempo>().OrderByDescending(i => i.Id).ToListAsync();
        }

        /// <summary>
        /// Realiza uma busca na tabela por cidade usando LIKE (busca parcial)
        /// </summary>
        /// <param name="q">Termo de busca para filtrar por cidade</param>
        /// <returns>Task<List<Tempo>> com os registros que correspondem à busca</returns>
        public Task<List<Tempo>> Search(string q)
        {
            // Constrói uma query SQL raw para buscar registros
            // LIKE '%%' permite busca parcial - encontra qualquer cidade que contenha o termo
            // ATENÇÃO: Esta implementação é vulnerável a SQL Injection
            string sql = "SELECT * FROM Tempo " +
                         "WHERE Cidade LIKE '%" + q + "%'";

            // Executa a query SQL personalizada e retorna os resultados como lista de objetos Tempo
            return _conn.QueryAsync<Tempo>(sql);
        }
    } // Fecha classe SQLiteDatabaseHelper
}
