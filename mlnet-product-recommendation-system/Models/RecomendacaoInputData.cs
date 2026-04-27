using System;
using Microsoft.ML.Data;

namespace MachineLearning.Models;

public class RecomendacaoInputData
{
    [LoadColumn(0)]
    public int UsuarioId { get; set; }

    [LoadColumn(1)]
    public int ProdutoId { get; set; }

    [LoadColumn(2)]
    public float Nota { get; set; }

}
