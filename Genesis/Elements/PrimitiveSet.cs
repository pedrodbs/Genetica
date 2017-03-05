using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Genesis.Util;

namespace Genesis.Elements
{
	public class PrimitiveSet : IDisposable
	{
        public static readonly Constant Zero = new Constant(0);
        public static readonly Constant One = new Constant(1);
        public static readonly PrimitiveSet Default = new PrimitiveSet(
            new HashSet<Terminal> {Zero,One}, 
            new HashSet<IFunction>
            {
                new AdditionFunction(Zero,Zero),
                new CosineFunction(Zero),
                new DivisionFunction(Zero, Zero),
                new IfFunction(Zero,Zero,Zero,Zero),
                new LogarithmFunction(Zero,Zero),
                new MaxFunction(Zero,Zero),
                new MinFunction(Zero,Zero),
                new MultiplicationFunction(Zero,Zero),
                new PowerFunction(Zero,Zero),
                new SineFunction(Zero),
                new SubtractionFunction(Zero,Zero),
            });

	    private bool _disposed;
	    private readonly HashSet<Terminal> _terminals;
        private readonly HashSet<IFunction> _functions;

	    public IReadOnlyCollection<Terminal> Terminals => this._terminals.ToList();
        public IReadOnlyCollection<IFunction> Functions => this._functions.ToList();

		public PrimitiveSet(IEnumerable<Terminal> terminals, IEnumerable<IFunction> operators)
		{
			this._terminals = new HashSet<Terminal>(terminals);
			this._functions = new HashSet<IFunction>(operators);
		}

	    public void Add(PrimitiveSet primitiveSet)
	    {
	        this._terminals.AddRange(primitiveSet.Terminals);
	        this._functions.AddRange(primitiveSet.Functions);
	    }

	    protected virtual void Dispose(bool disposing)
	    {
            if(this._disposed) return;
	        if (disposing)
	        {
                this._functions.Clear();
                this._terminals.Clear();
            }
	        this._disposed = true;
	    }

	    public void Dispose()
	    {
	        this.Dispose(true);
            GC.SuppressFinalize(this);
	    }
	}
}
