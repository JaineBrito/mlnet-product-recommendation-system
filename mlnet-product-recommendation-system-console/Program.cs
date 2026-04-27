using MachineLearning.ML;
using MachineLearning.Models;


ProjetoRecomendacao();

void ProjetoRecomendacao()
{
    var trainer = new RecomendacaoModelTrainer();
    trainer.CarregarDadosCSV(Path.Combine(AppContext.BaseDirectory, "recomendacoes.csv"));
    trainer.TreinarModelo();

    var pathModelo = Path.Combine(AppContext.BaseDirectory, "modelo_recomendacao.zip");
    trainer.SalvarModelo(pathModelo);

    var predictor = new RecomendacaoModelPredictor();
    predictor.CarregarModelo(pathModelo);

    var novaRecomendacao = new RecomendacaoInputData()
    {
        UsuarioId = 1,
        ProdutoId = 10
    };

    var resultado = predictor.Prever(novaRecomendacao);
    Console.WriteLine($"Score de recomendação: {resultado.Score}");
}