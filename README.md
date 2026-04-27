# ML.NET Product Recommendation System

Idioma: **Portugues** | [English](README.en.md)

Projeto de estudo em C# com ML.NET para recomendar produtos a usuarios com base em historico de interacoes (usuario, produto, nota).

O objetivo e praticar o fluxo completo de Machine Learning no .NET:
- carregar dados;
- treinar modelo de recomendacao;
- salvar/carregar modelo;
- prever score para novas combinacoes usuario-produto.

## Estrutura do projeto

- `mlnet-product-recommendation-system`  
  Biblioteca com a logica de ML (treino, persistencia e predicao).
- `mlnet-product-recommendation-system-console`  
  Aplicacao de console que executa o fluxo ponta a ponta.

### Arquivos principais

- `mlnet-product-recommendation-system-console/Program.cs`  
  Orquestra todo o processo (carregar dados -> treinar -> salvar -> carregar -> prever).
- `mlnet-product-recommendation-system/ML/RecomendacaoModelTrainer.cs`  
  Contem os metodos para carregar CSV, treinar com Matrix Factorization e salvar modelo.
- `mlnet-product-recommendation-system/ML/RecomendacaoModelPredictor.cs`  
  Carrega o modelo salvo e faz previsoes.
- `mlnet-product-recommendation-system/Models/RecomendacaoInputData.cs`  
  Define as colunas de entrada do CSV (`UsuarioId`, `ProdutoId`, `Nota`).
- `mlnet-product-recommendation-system/Models/RecomendacaoPredictionResult.cs`  
  Define a saida da predicao (`Score`).

## Como os dados sao lidos

O dataset CSV e carregado por `LoadFromTextFile<RecomendacaoInputData>()` com:
- cabecalho (`hasHeader: true`);
- separador virgula (`separatorChar: ','`).

Mapeamento de colunas no `RecomendacaoInputData`:
- coluna `0`: `UsuarioId`;
- coluna `1`: `ProdutoId`;
- coluna `2`: `Nota` (valor real usado no treino).

## Pipeline de treino

No `RecomendacaoModelTrainer`:

1. Converte `UsuarioId` para chave em `UsuarioIdEncoded` (`MapValueToKey`).
2. Converte `ProdutoId` para chave em `ProdutoIdEncoded` (`MapValueToKey`).
3. Treina recomendacao com `MatrixFactorization`, usando:
   - `labelColumnName`: `Nota`;
   - `matrixColumnIndexColumnName`: `UsuarioIdEncoded`;
   - `matrixRowIndexColumnName`: `ProdutoIdEncoded`.

## AutoML

Este projeto referencia `Microsoft.ML.AutoML`, mas o fluxo atual nao executa experimento AutoML no codigo.

## Salvar e carregar modelo

- `SalvarModelo(path)` salva o modelo treinado em arquivo `.zip`.
- `CarregarModelo(path)` recupera esse modelo para uso futuro.

Isso permite separar treino e inferencia.

## Predicao

A predicao e feita com:
- `CreatePredictionEngine<RecomendacaoInputData, RecomendacaoPredictionResult>()`
- `Predict(novaRecomendacao)`

Exemplo do projeto:
- entrada: combinacao `UsuarioId = 1` e `ProdutoId = 10`;
- saida: `Score` de recomendacao para esse par usuario-produto.

## Como executar

Na raiz do repositorio:

```bash
dotnet restore "mlnet-product-recommendation-system.sln"
dotnet build "mlnet-product-recommendation-system.sln" -c Debug
dotnet run --project "mlnet-product-recommendation-system-console/mlnet-product-recommendation-system-console.csproj"
```

## Dependencias

No projeto de biblioteca:
- `Microsoft.ML`
- `Microsoft.ML.AutoML`
- `Microsoft.ML.LightGbm`

## Observacoes de estudo

- O projeto e focado em aprendizado, nao em producao.
- Para evoluir, voce pode:
  - ajustar hiperparametros de Matrix Factorization;
  - aplicar split de treino/teste para medir qualidade;
  - adicionar mais sinais de comportamento (categoria, recencia, contexto);
  - comparar abordagem atual com outros algoritmos de recomendacao.