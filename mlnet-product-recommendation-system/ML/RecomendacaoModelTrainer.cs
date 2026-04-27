using System;
using MachineLearning.Models;
using Microsoft.ML;

namespace MachineLearning.ML;

public class RecomendacaoModelTrainer
{
    private MLContext mLContext = new MLContext();
    private IDataView dados;

    private ITransformer modeloTreinado;

    public void CarregarDadosCSV(string path)
    {
        dados = mLContext.Data.LoadFromTextFile<RecomendacaoInputData>(
            path: path,
            hasHeader: true,
            separatorChar: ','
        );
    }

    public void TreinarModelo()
    {
        // Criar o pipeline
        var pipeline = mLContext.Transforms.Conversion.MapValueToKey(
            "UsuarioIdEncoded", nameof(RecomendacaoInputData.UsuarioId))
        .Append(mLContext.Transforms.Conversion.MapValueToKey(
            "ProdutoIdEncoded", nameof(RecomendacaoInputData.ProdutoId)))
        .Append(mLContext.Recommendation().Trainers.MatrixFactorization(
            labelColumnName: nameof(RecomendacaoInputData.Nota),
            matrixColumnIndexColumnName: "UsuarioIdEncoded",
            matrixRowIndexColumnName: "ProdutoIdEncoded"
        ));

        // Treinar o modelo
        modeloTreinado = pipeline.Fit(dados);
    }

    public void SalvarModelo(string path)
    {
        mLContext.Model.Save(modeloTreinado, dados.Schema, path);
    }

}
