using Common.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Users
{
    public class InitUsuariosData
    {
        public InitUsuariosData()
        {
            UsuariosViewModel = new UsuariosData();
        }

        public UsuariosData UsuariosViewModel { get; set; }

    }
}
