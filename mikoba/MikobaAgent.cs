using System;
using Hyperledger.Aries.Agents;
using mikoba.CoreImplementations;

namespace mikoba
{
    public class MikobaAgent : AgentBase
    {
        public MikobaAgent(IServiceProvider provider) : base(provider)
        {
            
        }
    
        /// <summary>
        /// Configures the handlers.
        /// </summary>
        protected override void ConfigureHandlers()
        {
            try
            {
                this.Handlers.Add(new MikobaCredentialHandler());
                this.Handlers.Add(new MikobaProofHandler());
                base.AddConnectionHandler();
                base.AddDiscoveryHandler();
                base.AddBasicMessageHandler();
                base.AddForwardHandler();
                base.AddTrustPingHandler();
                
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
