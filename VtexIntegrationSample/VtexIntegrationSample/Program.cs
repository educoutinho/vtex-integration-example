using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestSendOrder();

            Console.WriteLine();
            Console.WriteLine("Bye!");
            Console.ReadKey();
        }

        private static void TestSendOrder()
        {
            var integration = VtexIntegration.Create();
            integration.LogEvent += Log;
            integration.LogServiceCallEvent += LogServiceCall;
            
            //Dados para teste
            string shippingTypeID = "SEDEX";
            
            //Itens para enviar no pedido
            var listItems = new List<Models.Item>();
            listItems.Add(new Models.Item("988", "7894854031548", "Máscara TNT Tipo Calcinha Branca SKY", null, null, null, null, null, null, "protcap"));
            
            //Dados do cliente (obtidos do arquivo de cnofig)
            var clientIntegration = GetClientIntegration();
            var deliveryAddressIntegration = GetClientDeliveryAddress();
            

            //---- simulation

            var responseGetItemsPrice = CheckCart(integration, clientIntegration, deliveryAddressIntegration, listItems);
            var jsonGetItemsPrice = Utils.JsonSerialize(responseGetItemsPrice);
                        
            //---- payment information

            //TODO: Você precisa consultar esses códigos na instalação da VTEX
            //1   American Express
            //2   Visa
            //3   Diners
            //4   Mastercard
            //8   Hipercard
            //9   Elo
            //1   American Express
            //2   Visa
            //3   Diners
            //4   Mastercard
            //8   Hipercard
            //9   Elo

            var paymentCondition = new Models.PaymentCondition("1", "American Express", "creditCardPaymentGroup", Models.PaymentTypesEnum.Amex, 0, new List<Models.PaymentInstallment>());
            var paymentConditionInformation = new Models.PaymentConditionInformation(paymentCondition, 0, 1, 0);
            
            paymentConditionInformation.SetCreditCardInformation("376600000000000", "Jose da Silva", 2022, 12, "0000", "66186378489");
            //Ei você debugando, rode esse comando novamente no Immediate Window enviando dados válidos de cartão
            System.Diagnostics.Debugger.Break();

            //-------- create order request

            int itemQuantity = 1;
            int installmentQuantity = 1;

            var requestCloseOrderListItems = new List<Models.SendOrderRequestItem>();
            var requestCloseOrderIntegrationListItems = new List<Models.SendOrderRequestItem>();

            var listShippingInformations = new List<Models.ShippingInformation>();
            foreach (var itemPriceIntegration in responseGetItemsPrice.ItemsList)
            {
                var shippingInformationIntegration = itemPriceIntegration.ShippingInformations.Where(a => a.ShippingTypeID == shippingTypeID).FirstOrDefault();
                if (shippingInformationIntegration == null)
                    throw new System.InvalidOperationException("Condição de entrega não encontrada para o item");

                var shippingInformation = new Models.ShippingInformation(shippingTypeID, shippingTypeID, itemPriceIntegration.Value, new Models.ShippingTime(shippingInformationIntegration.ShippingTime.Days, shippingInformationIntegration.ShippingTime.IsBusinessDays));

                var item = listItems.Where(a => a.Barcode == itemPriceIntegration.Barcode).FirstOrDefault();

                listShippingInformations.Add(shippingInformationIntegration);

                requestCloseOrderListItems.Add(new Models.SendOrderRequestItem(item.Barcode, itemPriceIntegration.SupplierItemCode, itemPriceIntegration.SellerCode, itemQuantity, itemPriceIntegration.Value, itemQuantity * itemPriceIntegration.Value, shippingInformation));
                requestCloseOrderIntegrationListItems.Add(new Models.SendOrderRequestItem(itemPriceIntegration.Barcode, itemPriceIntegration.SupplierItemCode, itemPriceIntegration.SellerCode, itemQuantity, itemPriceIntegration.Value, itemQuantity * itemPriceIntegration.Value, shippingInformationIntegration));
            }

            int totalQuantity = requestCloseOrderIntegrationListItems.Sum(a => a.Quantity);
            decimal subTotalValue = requestCloseOrderIntegrationListItems.Sum(a => a.Total);
            decimal shippingValue = requestCloseOrderIntegrationListItems.Sum(a => a.ShippingInformation.Price);
            decimal orderTotalValue = subTotalValue + shippingValue;
            decimal installmentsValue = orderTotalValue / (decimal)installmentQuantity;

            paymentConditionInformation.SetValue(orderTotalValue);
            paymentConditionInformation.SetInstallmentValue(installmentQuantity, installmentsValue);

            var paymentConditionIntegration = new Models.PaymentCondition(paymentCondition.PaymentConditionCode, paymentCondition.Name, paymentCondition.GroupName, paymentCondition.PaymentConditionTypeID, orderTotalValue, new List<Models.PaymentInstallment>());
            var paymentConditionInformationIntegration = new Models.PaymentConditionInformation(paymentConditionIntegration, paymentConditionInformation.Value, paymentConditionInformation.InstallmentQuantity, paymentConditionInformation.InstallmentValue);
            paymentConditionInformationIntegration.SetCreditCardInformation(paymentConditionInformation.CardNumber, paymentConditionInformation.HolderName, paymentConditionInformation.DueYear, paymentConditionInformation.DueMonth, paymentConditionInformation.ValidationCode, paymentConditionInformation.DocumentNumber);

            var listConsolidateShippingInformations = Utils.ConsolidateShippingInformation(responseGetItemsPrice.ItemsList);
            var consolidateShippingInformationIntegration = listConsolidateShippingInformations.Where(a => a.ShippingTypeID == shippingTypeID).FirstOrDefault();
            var consolidateShippingInformation = new Models.ShippingInformation(consolidateShippingInformationIntegration.ShippingTypeID, consolidateShippingInformationIntegration.Name, consolidateShippingInformationIntegration.Price, new Models.ShippingTime(consolidateShippingInformationIntegration.ShippingTime.Days, consolidateShippingInformationIntegration.ShippingTime.IsBusinessDays));

            var requestCloserOrder = new Models.SendOrderRequest(clientIntegration, orderTotalValue, requestCloseOrderListItems, deliveryAddressIntegration, paymentConditionInformation);
            var requestCloseOrderIntegration = new Models.SendOrderRequest(clientIntegration, orderTotalValue, requestCloseOrderIntegrationListItems, deliveryAddressIntegration, paymentConditionInformationIntegration);
            
            //----- send order
            System.Diagnostics.Debugger.Break();
            var responseCloseOrder = SendOrder(integration, requestCloseOrderIntegration);
            
            //----- save order
            //TODO: Salvar pedido no bd
            Log(string.Format("OrderID=0, SupplierOrderNumber={0}", responseCloseOrder.OrderNumber));

            //---- send payment
            System.Diagnostics.Debugger.Break();
            var responseSendPayment = SendPayment(integration, clientIntegration, responseCloseOrder.OrderNumber, responseCloseOrder.PaymentTransactionCode, paymentConditionInformationIntegration);

            //------ complete order
            System.Diagnostics.Debugger.Break();
            var responseCompleteOrder = integration.CompleteOrder(new Models.CompleteOrderRequest(clientIntegration, responseCloseOrder.OrderNumber));
                        
            //------ get payment information
            System.Diagnostics.Debugger.Break();
            var paymentStatus = GetPaymentStatus(integration, clientIntegration, responseCloseOrder.PaymentTransactionCode);
            
            System.Diagnostics.Debugger.Break();
        }
        
        private static Models.ClientIntegration GetClientIntegration()
        {
            string directoryPath = Utils.GetConfigDirectory();
            string path = System.IO.Path.Combine(directoryPath, "client.xml");
            
            Models.ClientIntegration client;

            if (!System.IO.File.Exists(path))
            {
                var billingAddress = new Models.Address("principal", "Jose Teste", "Av Paulista", "1000", "cj 77", "Cerqueira César", "01311200", "São Paulo", "SP", "BRA");
                client = new Models.ClientIntegration(0, "Empresa Teste", "Jose", "Teste", "14452872182", "96379179000109", "Isento", "teste@teste.com.br", "11 6666-6666", billingAddress);

                using (var streamWriter = new System.IO.StreamWriter(path))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(Models.ClientIntegration)).Serialize(streamWriter, client);
                }
            }

            try
            {
                using (var streamReader = new System.IO.StreamReader(path))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.ClientIntegration));
                    client = (Models.ClientIntegration)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException(string.Format("Erro na leitura do arquivo com os dados do cliente -- path: {0} -- {1}", path, ex.Message), ex);
            }

            return client;
        }
        
        private static Models.Address GetClientDeliveryAddress()
        {
            string directoryPath = Utils.GetConfigDirectory();
            string path = System.IO.Path.Combine(directoryPath, "clientDeliveryAddress.xml");

            Models.Address address;
            if (!System.IO.File.Exists(path))
            {
                address = new Models.Address("entrega", "Jose Teste", "Av Paulista", "1000", "cj 77", "Cerqueira César", "01311200", "São Paulo", "SP", "BRA");

                using (var streamWriter = new System.IO.StreamWriter(path))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(Models.Address)).Serialize(streamWriter, address);
                }
            }

            try
            {
                using (var streamReader = new System.IO.StreamReader(path))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.Address));
                    address = (Models.Address)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException(string.Format("Erro na leitura do arquivo com o endereço de faturamento -- path: {0} -- {1}", path, ex.Message), ex);
            }

            return address;
        }
        
        private static Models.GetItemsPriceResponse CheckCart(VtexIntegration integration, Models.ClientIntegration clientIntegration, Models.Address deliveryAddressIntegration, List<Models.Item> listItem)
        {
            int itemQuantity = 1;

            var requestItemsPriceListItems = new List<Models.ItemGetItemsPriceRequest>();
            foreach (var item in listItem)
                requestItemsPriceListItems.Add(new Models.ItemGetItemsPriceRequest(item.SupplierItemCode, item.Barcode, itemQuantity, item.SellerCode));

            var requestItemsPrice = new Models.GetItemsPriceRequest(clientIntegration, deliveryAddressIntegration.Zipcode, requestItemsPriceListItems, null);
            var responseItemsPrice = integration.GetItemsPrice(requestItemsPrice);

            return responseItemsPrice;
        }

        private static Models.SendOrderResponse SendOrder(VtexIntegration integration, Models.SendOrderRequest requestCloseOrderIntegration)
        {
            var responseCloseOrderIntegration = integration.SendOrder(requestCloseOrderIntegration);

            if (responseCloseOrderIntegration.Status != Models.SendOrderStatusEnum.Success)
            {
                System.Diagnostics.Debugger.Break();
                throw new System.InvalidOperationException(string.Format("Não foi possível criar o pedido -- {0}", responseCloseOrderIntegration.Message));
            }

            Console.WriteLine(string.Format("responseCloseOrder -- status: {0}, response: {1}", responseCloseOrderIntegration.Status.ToString(), Utils.JsonSerialize(responseCloseOrderIntegration, true, true)));

            return responseCloseOrderIntegration;
        }
        
        private static Models.SendPaymentResponse SendPayment(VtexIntegration integration, Models.ClientIntegration clientIntegration, string supplierOrderNumber, string supplierTransactionCode, Models.PaymentConditionInformation paymentConditionInformation)
        {
            var request = new Models.SendPaymentRequest(clientIntegration, supplierOrderNumber, supplierTransactionCode, paymentConditionInformation);
            var response = integration.SendPayment(request);
            return response;
        }

        private static Models.GetPaymentStatusResponse GetPaymentStatus(VtexIntegration integration, Models.ClientIntegration clientIntegration, string supplierTransactionCode)
        {
            var request = new Models.GetPaymentStatusRequest(clientIntegration, supplierTransactionCode);
            var response = integration.GetPaymentStatus(request);
            return response;
        }

        #region Auxiliar Methods

        private static void Log(string message)
        {
            Console.WriteLine(Utils.MaxLength(message, 600, "..."));
            System.Diagnostics.Debug.WriteLine(message);

            string path = System.IO.Path.Combine(Utils.GetExecutableFolderPath(System.Reflection.Assembly.GetExecutingAssembly()), @"_logSample.txt");
            using (var streamWriter = new System.IO.StreamWriter(path, true))
            {
                streamWriter.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            }
        }

        private static void LogServiceCall(string message)
        {
            Console.WriteLine(Utils.MaxLength(message, 600, "..."));

            string path = System.IO.Path.Combine(Utils.GetExecutableFolderPath(System.Reflection.Assembly.GetExecutingAssembly()), @"_logSample.txt");
            using (var streamWriter = new System.IO.StreamWriter(path, true))
            {
                streamWriter.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            }
        }
        
        #endregion
    }
}
