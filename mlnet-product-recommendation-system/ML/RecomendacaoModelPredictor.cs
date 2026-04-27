using System;
using MachineLearning.Models;
using Microsoft.ML;

namespace MachineLearning.ML;

public class RecomendacaoModelPredictor
{
    private MLContext mLContext = new MLContext();
    private ITransformer modeloCarregado;

    public void CarregarModelo(string path)
    {
        DataViewSchema modeloSchema;
        modeloCarregado = mLContext.Model.Load(path, out modeloSchema);
    }

    public RecomendacaoPredictionResult Prever(RecomendacaoInputData novaRecomendacao)
    {
        var predEngine = mLContext.Model.CreatePredictionEngine<RecomendacaoInputData,
        RecomendacaoPredictionResult>(modeloCarregado);
        return predEngine.Predict(novaRecomendacao);
    }

}
