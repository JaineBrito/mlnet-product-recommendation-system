# ML.NET Product Recommendation System

Language: [Portugues](README.md) | **English**

Study project in C# with ML.NET to recommend products to users based on interaction history (user, product, rating).

The goal is to practice the full Machine Learning workflow in .NET:
- load data;
- train a recommendation model;
- save/load model artifacts;
- predict scores for new user-product pairs.

## Project structure

- `mlnet-product-recommendation-system`  
  Class library with ML logic (training, persistence, and prediction).
- `mlnet-product-recommendation-system-console`  
  Console app that runs the end-to-end workflow.

### Main files

- `mlnet-product-recommendation-system-console/Program.cs`  
  Orchestrates the full process (load data -> train -> save -> load -> predict).
- `mlnet-product-recommendation-system/ML/RecomendacaoModelTrainer.cs`  
  Contains methods to load CSV, train with Matrix Factorization, and save the model.
- `mlnet-product-recommendation-system/ML/RecomendacaoModelPredictor.cs`  
  Loads the saved model and performs predictions.
- `mlnet-product-recommendation-system/Models/RecomendacaoInputData.cs`  
  Defines CSV input columns (`UsuarioId`, `ProdutoId`, `Nota`).
- `mlnet-product-recommendation-system/Models/RecomendacaoPredictionResult.cs`  
  Defines prediction output (`Score`).

## How data is loaded

The CSV dataset is loaded with `LoadFromTextFile<RecomendacaoInputData>()` using:
- header row (`hasHeader: true`);
- comma separator (`separatorChar: ','`).

Column mapping in `RecomendacaoInputData`:
- column `0`: `UsuarioId`;
- column `1`: `ProdutoId`;
- column `2`: `Nota` (ground-truth value used during training).

## Training pipeline

Inside `RecomendacaoModelTrainer`:

1. Convert `UsuarioId` into keyed `UsuarioIdEncoded` (`MapValueToKey`).
2. Convert `ProdutoId` into keyed `ProdutoIdEncoded` (`MapValueToKey`).
3. Train recommendation with `MatrixFactorization`, using:
   - `labelColumnName`: `Nota`;
   - `matrixColumnIndexColumnName`: `UsuarioIdEncoded`;
   - `matrixRowIndexColumnName`: `ProdutoIdEncoded`.

## AutoML

This project references `Microsoft.ML.AutoML`, but the current workflow does not run an AutoML experiment in code.

## Save and load model

- `SalvarModelo(path)` saves the trained model to a `.zip` file.
- `CarregarModelo(path)` loads that model for future use.

This allows training and inference to be separated.

## Prediction

Prediction is done with:
- `CreatePredictionEngine<RecomendacaoInputData, RecomendacaoPredictionResult>()`
- `Predict(novaRecomendacao)`

Project example:
- input: pair `UsuarioId = 1` and `ProdutoId = 10`;
- output: recommendation `Score` for that user-product pair.

## How to run

From repository root:

```bash
dotnet restore "mlnet-product-recommendation-system.sln"
dotnet build "mlnet-product-recommendation-system.sln" -c Debug
dotnet run --project "mlnet-product-recommendation-system-console/mlnet-product-recommendation-system-console.csproj"
```

## Dependencies

In the class library project:
- `Microsoft.ML`
- `Microsoft.ML.AutoML`
- `Microsoft.ML.LightGbm`

## Study notes

- This project is learning-focused, not production-ready.
- To evolve it further, you can:
  - tune Matrix Factorization hyperparameters;
  - add a train/test split to measure quality;
  - include more behavioral signals (category, recency, context);
  - compare this approach with other recommendation algorithms.