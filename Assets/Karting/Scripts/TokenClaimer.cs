using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;

public class TokenClaimer : MonoBehaviour
{
    private ThirdwebSDK sdk;

    public GameObject balanceText;

    public GameObject claimButton;

    async void OnEnable()
    {
        sdk =
            new ThirdwebSDK("optimism-goerli",
                new ThirdwebSDK.Options()
                {
                    gasless =
                        new ThirdwebSDK.GaslessOptions()
                        {
                            openzeppelin =
                                new ThirdwebSDK.OZDefenderOptions()
                                {
                                    relayerUrl =
                                        "https://api.defender.openzeppelin.com/autotasks/c2e9a6ca-f2e8-4521-926b-1f9daec2dcb8/runs/webhook/826a5b67-d55d-49dc-8651-5db958ba22b2/DPtceJtayVGgKSDejaFnWk"
                                }
                        }
                });

        await ConnectWallet();
        CheckBalance();
    }

    public async void Claim()
    {
        await ConnectWallet();
        await getTokenDrop().ERC20.Claim("25");

        // hide claim button
        claimButton.SetActive(false);

        // Run OnEnable again to update balance
        OnEnable();
    }

    private Contract getTokenDrop()
    {
        return sdk.GetContract("0x4a9659d5E0d416Ce8B9a4336132012Af8db4c5AB");
    }

    private async Task<string> ConnectWallet()
    {
        return await sdk
            .wallet
            .Connect(new WalletConnection()
            {
                provider = WalletProvider.CoinbaseWallet, // Use Coinbase Wallet
                chainId = 420 // Switch the wallet Goerli network on connection
            });
    }

    private async void CheckBalance()
    {
        await ConnectWallet();

        // Set text to user's balance
        var bal = await getTokenDrop().ERC20.Balance();

        balanceText.GetComponent<TMPro.TextMeshProUGUI>().text =
            bal.displayValue + " " + bal.symbol;
    }
}
