using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class Fs
    {
        public static Error PathNotFound => Error.NotFound(
            code: "Fs.PathNotFound",
            description: "The item with specified path hasn't been found. Did You Forget to put / at the end?");
    }
}
