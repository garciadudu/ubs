using System.Globalization;
using Ub;


namespace Ub 
{ 
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Para importar o arquivo");
                Console.WriteLine("I)NomeDoArquivo.ext");
                Console.WriteLine("S)air");

                var comando = Console.ReadLine();

                Thread.Sleep(1000);

                if (comando == "S")
                {
                    Console.WriteLine("Saindo da aplicação");
                    break;
                }
                else if (comando != "" && comando?[0] == 'I')
                {
                    Console.WriteLine("Importando o arquivo");
                    // LENDO O ARQUIVO

                    var arquivo = comando.Substring(1, comando.Length - 1);


                    StreamReader sr = new StreamReader(arquivo);
                    var line = sr.ReadLine();


                    Console.WriteLine("Exemplo de entrada");


                    DateTime PrimeiraData;

                    var valido = DateTime
                        .TryParseExact(line,
                                        "MM/dd/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                                        out PrimeiraData);

                    if (!valido)
                    {
                        Console.WriteLine("Arquivo Inválido");
                        sr.Close();
                        continue;
                    }

                    Console.WriteLine(PrimeiraData);

                    line = sr.ReadLine();

                    int validoInt;

                    valido = int.TryParse(line,
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out validoInt);

                    if (!valido)
                    {
                        Console.WriteLine("Arquivo Inválido");
                        sr.Close();
                        continue;
                    }

                    Console.WriteLine(validoInt);

                    var lista = new List<Trade>();

                    for (var x = 0; x < validoInt; x++)
                    {
                        line = sr.ReadLine();

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        var minhaLinha = line.Split(" ");

                        if (string.IsNullOrEmpty(minhaLinha[0]))
                        {
                            Console.WriteLine("linha {0} inválida", x + 1);
                            continue;
                        }

                        long validoLong;

                        valido = long.TryParse(minhaLinha[0],
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out validoLong);

                        if (!valido)
                        {
                            Console.WriteLine("Valor inválido na linha {0}", x + 1);
                            continue;
                        }

                        if (minhaLinha[1] != "Private" && minhaLinha[1] != "Public")
                        {
                            Console.WriteLine("Valor inválido na linha {0}, Setor do cliente só pode ser 'Public' ou 'Private'", x + 1);
                            continue;
                        }

                        DateTime NovaData;

                        valido = DateTime
                            .TryParseExact(minhaLinha[2],
                                            "MM/dd/yyyy",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out NovaData);

                        if (!valido)
                        {
                            Console.WriteLine("Data inválida na linha {0}, formato 'MM/dd/yyyy'", x + 1);
                            continue;
                        }


                        var item = new Trade(Convert.ToDouble(validoLong), minhaLinha[1], NovaData);

                        lista.Add(item);

                        Console.WriteLine(line);
                    }

                    sr.Close();

                    Console.WriteLine("");

                    Console.WriteLine("Exemplo de saída");

                    foreach (var item in lista)
                    {

                        if (item.NextPaymentDate < PrimeiraData.AddDays(-30))
                        {
                            Console.WriteLine("EXPIRED");
                        }

                        if ((item.Value >= 1000000) && (item.ClientSector == "Private"))
                        {
                            Console.WriteLine("HIGHRISK");
                        }
                        else if ((item.Value >= 1000000) && (item.ClientSector == "Public"))
                        {
                            Console.WriteLine("MEDIUMRISK");
                        }
                    }


                    Thread.Sleep(5000);
                }
            }
        }
    }

}