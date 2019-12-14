using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Majestic.Models;

public class MyContractProvider
{
    IContractRepository _rep;

    public MyContractProvider()
    {
        _rep = new ContractRepository();
    }

    public MyContractProvider(IContractRepository rep)
    {
        _rep = rep;
    }

    public void SetContractRepository(IContractRepository rep)
    {
       _rep = rep;
    }

    public void SaveContract(string text)
    {
        _rep.SaveContract(text);
    }

    public void Delete(int id)
    {
        _rep.Delete(id);
    }

    public void Update(int id)
    {
        _rep.Update(id);
    }

}

