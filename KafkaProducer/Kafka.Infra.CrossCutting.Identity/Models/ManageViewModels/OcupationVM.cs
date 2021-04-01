using System;
using System.ComponentModel.DataAnnotations;

namespace Kafka.Infra.CrossCutting.Identity.Models.ManageViewModels
{
    public class OcupationVM
    {

        public int Key { get; set; }

        public string Description { get; set; }

    }
}