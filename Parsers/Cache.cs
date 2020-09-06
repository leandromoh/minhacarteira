using System;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Reflection;

namespace consoleapp
{
    public static class Cache
    {
        public static IReadOnlyDictionary<string, Ativo> GetOrCreate(Func<IEnumerable<string>> factory, TipoAtivo tipo)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, $"{tipo}.json");
            IEnumerable<string> ativos;

            if (File.Exists(path))
            {
                using var streamFile = new StreamReader(path);
                var content = streamFile.ReadToEnd();
                ativos = JsonSerializer.Deserialize<string[]>(content);
            }
            else
            {
                ativos = factory();
                var content = JsonSerializer.Serialize(ativos);
                File.WriteAllText(path, content);
            }

            return ativos
                    .Select(x => new Ativo { Ticker = x, Tipo = tipo })
                    .ToDictionary(x => x.Ticker);
        }
    }
}