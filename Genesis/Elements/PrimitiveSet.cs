using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;

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

	    public IReadOnlyCollection<Terminal> Terminals { get; }

		public IReadOnlyCollection<IFunction> Functions { get; }

		public PrimitiveSet(IEnumerable<Terminal> terminals, IEnumerable<IFunction> operators)
		{
			this.Terminals = terminals.ToList();
			this.Functions = operators.ToList();
		}

	    protected virtual void Dispose(bool disposing)
	    {
            if(this._disposed) return;
	        if (disposing)
	        {
                ((List<IFunction>)this.Functions).Clear();
                ((List<Terminal>)this.Terminals).Clear();
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
