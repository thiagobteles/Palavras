Console.WriteLine("Quantidade de letras? ");
var xQuantidade = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Quantidade mínima? ");
var xQuantidadeMinima = Convert.ToInt32(Console.ReadLine());

var xDicionario = new Dictionary<int, string>();
for (var contador = 0; contador < xQuantidade; contador++)
{
    Console.WriteLine($"Informe a {contador + 1}ª letra: ");
    xDicionario.Add(contador + 1, Console.ReadLine() ?? "");
}

var xTodasCombinacoes = new List<int>();
for (var contador = xQuantidadeMinima; contador <= xQuantidade; contador++)
{
    xTodasCombinacoes.AddRange(ObterTodasCombinacoes(contador, xDicionario));
}

var xTodasPalavras = new List<string>();

foreach (var item in xTodasCombinacoes)
{
    var xItens = item.ToString().ToCharArray();
    var xPalavraMontada = "";
    for (var contador = 0; contador < xItens.Length; contador++)
    {
        xPalavraMontada += xDicionario[Convert.ToInt32(xItens[contador].ToString())];
    }

    xTodasPalavras.Add(xPalavraMontada);
}

var xListaFinal = await ObterPalavrasValidasAsync(xTodasPalavras);
for (var contador = xQuantidadeMinima; contador <= xQuantidade; contador++)
{
    Console.WriteLine();
    Console.WriteLine($"**  Palavras com {contador} letras  **");
    Console.WriteLine(string.Join("\n", xListaFinal.Where(x => x.Length == contador).OrderBy(x => x)));
    Console.WriteLine();
    Console.WriteLine();
}

Console.WriteLine("Digite alguma coisa para fechar a aplicação");
Console.ReadKey();

static List<int> ObterTodasCombinacoes(int aQuantidadeCaracter, Dictionary<int, string> aDicionario)
{
    var xListaFinal = aDicionario.Select(x => x.Key).ToList();
    var xContador = xListaFinal.Count;

    for (int xInterno = 1; xInterno <= aQuantidadeCaracter - 1; xInterno++)
    {
        var xPalavraTemporaria = new List<int>();
        for (int i = 1; i <= aDicionario.Count; i++)
        {
            for (int k = 1; k <= xListaFinal.Count; k++)
            {
                if (i != k && (xInterno == 1 || !xListaFinal[k - 1].ToString().Contains(i.ToString())))
                {
                    xPalavraTemporaria.Add(Convert.ToInt32($"{xListaFinal[k - 1]}{i}"));
                }
            }
        }

        xListaFinal = xPalavraTemporaria;
    }

    return xListaFinal;
}



static async Task<List<string>> ObterPalavrasValidasAsync(List<string> aListaCompleta)
{
    var xExistentes = await File.ReadAllLinesAsync("palavras.txt");
    var xPalavrasValidas =  new List<string>();

    var xOpcoes = new ParallelOptions
    {
        MaxDegreeOfParallelism = 100
    };

    await Parallel.ForEachAsync(aListaCompleta, xOpcoes, async (xAtual, xToken) =>
    {
        bool xValida = xExistentes.Contains(xAtual);
        if (xValida)
        {
            xPalavrasValidas.Add(xAtual);
        }
    });

    if (xPalavrasValidas != null && xPalavrasValidas.Count > 0)
    {
        return xPalavrasValidas.Distinct().ToList();
    }

    return [];
}