using GameHive.Core.Services;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;

namespace GameHive.Controllers
{
    public class PayPalController : Controller
    {
        private readonly PayPalService _payPalService;

        public PayPalController(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        public IActionResult CreatePayment(decimal amount)
        {
            var apiContext = _payPalService.GetAPIContext();

            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
            {
                new Transaction
                {
                    amount = new Amount
                    {
                        currency = "USD",
                        total = amount.ToString("F2")
                    },
                    description = "Game Purchase"
                }
            },
                redirect_urls = new RedirectUrls
                {
                    return_url = Url.Action("ExecutePayment", "PayPal", null, Request.Scheme),
                    cancel_url = Url.Action("CancelPayment", "PayPal", null, Request.Scheme)
                }
            };

            var createdPayment = payment.Create(apiContext);
            var approvalUrl = createdPayment.links.FirstOrDefault(l => l.rel == "approval_url")?.href;

            return Redirect(approvalUrl);
        }

        public IActionResult ExecutePayment(string paymentId, string token, string PayerID)
        {
            var apiContext = _payPalService.GetAPIContext();
            var paymentExecution = new PaymentExecution { payer_id = PayerID };
            var payment = new Payment { id = paymentId };
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            if (executedPayment.state.ToLower() == "approved")
            {
                return RedirectToAction("OrderConfirmation", "Orders");
            }

            return RedirectToAction("PaymentFailed");
        }

        public IActionResult CancelPayment()
        {
            return View();
        }
    }
}
