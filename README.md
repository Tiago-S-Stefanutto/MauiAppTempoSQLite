# MauiAppTempoSQLite

Um aplicativo .NET MAUI para consulta e armazenamento de previsões meteorológicas usando SQLite local e API OpenWeatherMap.

## 📱 Descrição

O **MauiAppTempoSQLite** é um aplicativo multiplataforma que permite aos usuários:
- Consultar previsões meteorológicas em tempo real
- Armazenar histórico de consultas localmente
- Pesquisar e filtrar previsões salvas
- Gerenciar dados meteorológicos offline

## 🚀 Tecnologias Utilizadas

- **.NET 8.0** - Framework base
- **.NET MAUI** - Interface multiplataforma
- **SQLite** - Banco de dados local
- **OpenWeatherMap API** - Dados meteorológicos
- **Newtonsoft.Json** - Processamento de JSON
- **XAML** - Interface do usuário

## 🎯 Plataformas Suportadas

- 📱 **Android** (API 21+)
- 🍎 **iOS** (11.0+)
- 💻 **Windows** (10.0.17763.0+)
- 🖥️ **macOS** (MacCatalyst 13.1+)

## 🏗️ Arquitetura do Projeto

```
MauiAppTempoSQLite/
├── 📱 App.xaml(.cs)           # Aplicação principal
├── 🏠 MainPage.xaml(.cs)      # Página principal
├── 🖼️ AppShell.xaml(.cs)      # Shell de navegação
├── 📊 Models/
│   └── Tempo.cs               # Model de dados meteorológicos
├── 🔧 Services/
│   └── Services.cs            # Serviço da API OpenWeatherMap
├── 🗄️ Helpers/
│   └── SQLiteDatabaseHelper.cs # Helper do banco SQLite
└── 📱 Platforms/              # Configurações específicas por plataforma
```

## 🎨 Funcionalidades

### ✨ Principais Recursos

- **🌤️ Consulta Meteorológica**: Busca dados em tempo real da API OpenWeatherMap
- **💾 Armazenamento Local**: Salva histórico no banco SQLite
- **🔍 Busca e Filtros**: Pesquisa previsões por cidade
- **📋 Listagem Organizada**: Visualização em lista com cabeçalhos
- **🗑️ Gerenciamento**: Remoção de registros com confirmação
- **🔄 Atualização**: Pull-to-refresh (preparado para implementação)

### 📋 Informações Exibidas

- **🏙️ Cidade**: Nome da localização
- **🌡️ Temperaturas**: Mínima e máxima
- **📍 Coordenadas**: Latitude e longitude
- **💨 Vento**: Velocidade
- **👁️ Visibilidade**: Condições atmosféricas
- **🌅 Sol**: Horários de nascer e pôr do sol
- **📅 Data**: Timestamp da consulta

## 🛠️ Configuração e Instalação

### Pré-requisitos

- Visual Studio 2022 com workload .NET MAUI
- .NET 8.0 SDK
- Chave da API OpenWeatherMap

### 🔧 Configuração

1. **Clone o repositório**
```bash
git clone [url-do-repositorio]
cd MauiAppTempoSQLite
```

2. **Configure a API Key**
```csharp
// Em Services/Services.cs, linha 23
string chave = "SUA_CHAVE_OPENWEATHERMAP_AQUI";
```

3. **Restaure os pacotes NuGet**
```bash
dotnet restore
```

4. **Execute o projeto**
```bash
dotnet build
dotnet run
```

## 🔑 Obtenção da Chave da API

1. Acesse [OpenWeatherMap](https://openweathermap.org/api)
2. Crie uma conta gratuita
3. Gere sua chave da API
4. Substitua no código conforme indicado acima

## 📊 Estrutura do Banco de Dados

### Tabela: `Tempo`

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | INTEGER | Chave primária (auto-increment) |
| `Cidade` | TEXT | Nome da cidade |
| `DataConsulta` | DATETIME | Data/hora da consulta |
| `lat` | REAL | Latitude |
| `lon` | REAL | Longitude |
| `temp_min` | REAL | Temperatura mínima |
| `temp_max` | REAL | Temperatura máxima |
| `visibility` | INTEGER | Visibilidade |
| `speed` | REAL | Velocidade do vento |
| `main` | TEXT | Condição principal |
| `description` | TEXT | Descrição detalhada |
| `sunrise` | TEXT | Horário do nascer do sol |
| `sunset` | TEXT | Horário do pôr do sol |

## 🎯 Como Usar

### 1. **Consultar Previsão**
- Digite o nome da cidade na barra de busca
- Toque em "Buscar Previsão"
- Os dados serão salvos automaticamente

### 2. **Pesquisar Histórico**
- Use a barra de pesquisa para filtrar por cidade
- A busca é feita em tempo real

### 3. **Remover Registros**
- Toque e segure um item na lista
- Selecione "Remover" no menu de contexto
- Confirme a exclusão

## 🔍 Detalhes Técnicos

### Padrões Utilizados

- **📋 MVVM**: Model-View-ViewModel pattern
- **🏗️ Code-Behind**: Lógica de apresentação
- **🔄 Data Binding**: Vinculação automática de dados
- **📦 Service Layer**: Separação de responsabilidades

### Componentes Principais

#### 🗄️ SQLiteDatabaseHelper
- Gerencia operações CRUD no banco SQLite
- Métodos assíncronos para melhor performance
- Criação automática da tabela

#### 🌐 DataService
- Consome API OpenWeatherMap
- Processa JSON de resposta
- Converte timestamps Unix

#### 📱 MainPage
- Interface principal do usuário
- Gerenciamento de estado
- Tratamento de eventos

## ⚠️ Pontos de Atenção

### 🔒 Segurança
- **API Key**: Atualmente hardcoded (mover para configuração segura)
- **SQL Injection**: Método `Search()` é vulnerável

### 🐛 Problemas Conhecidos
- **Deadlock**: Uso de `.Result` em operações assíncronas
- **Funcionalidades Incompletas**: Pull-to-refresh não implementado
- **DatePickers**: Sem funcionalidade de filtro por data

### 🔧 Melhorias Sugeridas

1. **Segurança**
   - Mover API key para configuração
   - Implementar queries parametrizadas

2. **Performance**
   - Remover uso de `.Result`
   - Implementar cache de dados

3. **Funcionalidades**
   - Completar pull-to-refresh
   - Adicionar filtro por data
   - Navegação para detalhes

## 📞 Suporte

Para dúvidas, sugestões ou problemas:
- Abra uma issue no repositório
- Entre em contato com a equipe de desenvolvimento

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

---

**Desenvolvido com ❤️ usando .NET MAUI**
