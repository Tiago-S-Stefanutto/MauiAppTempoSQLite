using System.Collections.ObjectModel;
using System.Diagnostics;
using MauiAppTempoSQLite.Models;
using MauiAppTempoSQLite.Services;

namespace MauiAppTempoSQLite
{
    /// <summary>
    /// Code-behind da MainPage - contém a lógica de apresentação da interface
    /// Herda de ContentPage e implementa o padrão Code-Behind do XAML
    /// </summary>
    public partial class MainPage : ContentPage
    {
        // ObservableCollection notifica automaticamente a UI sobre mudanças na coleção
        // Essencial para Data Binding - quando itens são adicionados/removidos, ListView atualiza
        ObservableCollection<Tempo> lista = new ObservableCollection<Tempo>();

        /// <summary>
        /// Construtor da página - executado quando a página é criada
        /// </summary>
        public MainPage()
        {
            // Inicializa todos os componentes definidos no XAML
            // Conecta eventos, aplica estilos, etc.
            InitializeComponent();

            // Conecta a ObservableCollection como fonte de dados da ListView
            // Data Binding: ListView automaticamente exibe itens da 'lista'
            lst_previsoes_tempo.ItemsSource = lista;
        }

        /// <summary>
        /// Método do ciclo de vida - executado toda vez que a página aparece na tela
        /// Override do método da classe base ContentPage
        /// </summary>
        protected async override void OnAppearing()
        {
            try
            {
                // Limpa a lista atual para evitar duplicatas
                lista.Clear();

                // PROBLEMA: .Result em operação assíncrona pode causar deadlock
                // Busca todos os registros do banco e adiciona na ObservableCollection
                // ForEach percorre cada item retornado e adiciona na lista
                App.Db.GetAll().Result.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                // Exibe diálogo de erro para o usuário
                // DisplayAlert é método assíncrono da ContentPage
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        /// <summary>
        /// Event handler do botão "Buscar Previsão"
        /// Conectado no XAML via Clicked="Button_Clicked"
        /// </summary>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se o usuário digitou algo no SearchBar
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    // Chama o serviço para buscar dados da API OpenWeatherMap
                    // Operação assíncrona - não bloqueia a UI
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        // Preenche campos que não vêm da API
                        t.Cidade = txt_cidade.Text; // Nome da cidade digitado pelo usuário
                        t.DataConsulta = DateTime.Now; // Timestamp da busca

                        // Monta string com dados da previsão (código comentado)
                        string dados_previsao = "";
                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n";

                        // Comentado: exibir dados no Label
                        //lbl_res.Text = dados_previsao;

                        // Salva o registro no banco de dados SQLite
                        await App.Db.Insert(t);

                        // Comentado: contagem de registros
                        //string msg = $"Total de {total_registros} registros salvos!";
                        //await DisplayAlert("Sucesso", msg, "OK");

                        // Atualiza a lista na interface
                        // PROBLEMA: Mesmo .Result que pode causar deadlock
                        lista.Clear();
                        App.Db.GetAll().Result.ForEach(i => lista.Add(i));
                    }
                    else
                    {
                        // API retornou null - cidade não encontrada ou erro
                        lbl_res.Text = "Sem dados de Previsão";
                    } // Fecha if t=null

                }
                else
                {
                    // Usuario não digitou nome da cidade
                    lbl_res.Text = "Preencha a cidade.";
                } // fecha if string is null or empty

            }
            catch (Exception ex)
            {
                // Trata qualquer erro durante o processo
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        /// <summary>
        /// Event handler para pull-to-refresh da ListView
        /// Conectado no XAML via Refreshing="lst_previsoes_tempo_Refreshing"
        /// MÉTODO VAZIO - funcionalidade não implementada
        /// </summary>
        private void lst_previsoes_tempo_Refreshing(object sender, EventArgs e)
        {
            // TODO: Implementar lógica de refresh
            // Deveria recarregar dados e definir IsRefreshing = false
        }

        /// <summary>
        /// Event handler para seleção de item na ListView
        /// Conectado no XAML via ItemSelected="lst_previsoes_tempo_ItemSelected"
        /// MÉTODO VAZIO - funcionalidade não implementada
        /// </summary>
        private void lst_previsoes_tempo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // TODO: Implementar ação ao selecionar item
            // Poderia navegar para página de detalhes ou mostrar popup
        }

        /// <summary>
        /// Event handler para mudança de texto no SearchBar de filtro
        /// Conectado no XAML via TextChanged="txt_search_TextChanged"
        /// Implementa busca em tempo real conforme usuário digita
        /// </summary>
        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // e.NewTextValue contém o novo texto digitado
                string q = e.NewTextValue;

                // Limpa lista atual
                lista.Clear();

                // Busca no banco usando o texto como filtro
                // Método Search busca por cidade que contenha o texto
                List<Tempo> tmp = await App.Db.Search(q);

                // Adiciona resultados da busca na ObservableCollection
                // ListView atualiza automaticamente devido ao Data Binding
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                // Trata erros durante a busca
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        /// <summary>
        /// Event handler para menu de contexto "Remover"
        /// Conectado no XAML via Clicked="MenuItem_Clicked"
        /// Permite deletar registros com confirmação
        /// </summary>
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Cast do sender para recuperar o MenuItem clicado
                MenuItem selecinado = sender as MenuItem;

                // BindingContext contém o objeto Tempo associado ao item da lista
                // Isso permite identificar qual registro foi selecionado
                Tempo t = selecinado.BindingContext as Tempo;

                // Exibe diálogo de confirmação antes de deletar
                // DisplayAlert com dois botões retorna true/false
                bool confirm = await DisplayAlert(
                    "Tem Certeza?", $"Remover previsão para {t.Cidade}?", "Sim", "Não");

                if (confirm)
                {
                    // Remove do banco de dados usando o ID
                    await App.Db.Delete(t.Id);

                    // Remove da ObservableCollection
                    // ListView atualiza automaticamente
                    lista.Remove(t);
                }
            }
            catch (Exception ex)
            {
                // Trata erros durante a remoção
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}