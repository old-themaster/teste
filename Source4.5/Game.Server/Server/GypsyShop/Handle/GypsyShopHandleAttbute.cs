using System;

namespace Game.Server.GypsyShop.Handle
{
    class GypsyShopHandleAttbute : Attribute
    {
        public byte Code { get; private set; }

        public GypsyShopHandleAttbute(byte code)
        {
            Code = code;
        }
    }
}