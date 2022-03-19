#!bin/bash

###################################################################################
#                             CALL AIS BALANCE  					              #
###################################################################################
# This script calls the AIS API in sandbox and performs a get request to the-     #
# endpoint "v3/{account-id}/balances" for account balance. You must request       #
# customer access token to call this script.   									  #
# Please update the variables "accessToken" and "certPath".   					  #
###################################################################################

keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
certPath="./certs/" # path of the downloaded certificates and keys
httpHost="https://api.sandbox.ing.com"

# httpMethod value must be in lower case
httpMethod="get"
reqPath="/v3/accounts/a217d676-7559-4f2a-83dc-5da0c2279223/balances?balanceTypes=interimBooked"

# Digest value for an empty body
digest="SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="

# Generated value of the customer access token. Please note that the customer access token expires in 15 minutes
caccessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..ghsZyJmVO821V0g3-G3xhg.e7tlZxmaZrcDiJDvn74esOIXlSKaTf9JUWIOt6sf-uH8pGyDaAggZ1BRZmv9w-4okLAgO_QwNhGLO8SFECpHN-F5PhvU8hjjDUT68HLI0XXdL65hahZ4ph0MQYEQ2iVYjDpVXVfxwQYe36VILPDwK-imnliqCRrF5EXEtXGHplL0MLNTmxgbQuQoWmL4BwNR3yELYy11CtsWKhL7KTmQjsbKVF5aHmTO5oPp_0WIrq80YW-6sKspsLY5O97gg72WNFj_pbu_Mx7vTPFDesD7z6X7Ciu7voRMeo61j1jnB8YgThkpVH-PxGD7Km-jNqt-YE349onr_KfQvSfA5TI6JN9JiYlfHNOP2AoNm6IPpGTxAffMs41VXriM4ioEklSbNH8sr1Ah-vh__mdCm4i3sNM-T0Z28uveuZhL0glvOv5IhZZHBqWws52Jm2Wv6g4Vr_XaBUHPxWcoHgZjj3zAScxXjZmlC5-gBSfz50xgkCV9u5mtGcshzDLYH90-_l85Izz6Rxw_W4CdSZFzHVWRXktvqRnaZzqWJ2B-LzxxhjV3mKM25vqg4uG8bnjcCesy6d91nAtNI6PMMcLECFKzMx2gQY1e61C7-GIDz4w2gQhwNJN2MAW860j8MqdE85rh_hOHcFGW3gUi4HiAbaD65ccpj0t-R7b3lUmbDm5B2HN06dQAGV8QDonSALzSMGpYkd-bdATN9_3dJZ4Wd_UWUzJCokD78jBNphxHv6EHLHLUrw1i7CRyGOiwWEN0_OSkMO8Pnr51z8Oj58xrQr-XrP49PasAu1LRRxVJw48YdAxQhyxORzK1MR3EfPmcOGWoeeLStiZTW35dJIl6icf2lwSFXE7BumVUAW3k8m4b6S9LZnBmjRrAa-cwTAfdJ46gY-spXqL7kxGPHYQ9SQHfj6UHUJnOg1_j1JwUpersf3QSmJ01dHZ34iiFoYvLWs5HEM135lO89HQ8XBfv095pvDlDVH8RDZ8zw_uevlAbV7qCh660bCQeAqwque6o4hzexVtpbQDeHVtIwQ9FbvF8M6jnbm-ljxyDGMTc511U3MZ1wv5hZzaom0jXdb3LKVAwVeT-fdlBn0ykDNV6tOUtqe3GDFm526VxAN8TcAh36wSkN9cNNwdIFZU39j_SOzWUbyIrQbk-loNCyXiQb8OmE8VmQdrzjrJEiJ3_cv2bOLOdn4FH8-ag_5OAae9VM0XjWTHVpv88l3Bxb8Nf1snyrb2bDn-1KbpcJrjzpnSJW3nZ0FTBONBkVpfZ3VBNtOPjc-7-_WPTZIJb6k1GZJhFv5AsFXG40HaHl-pyWnQlXCXsaqNdVm4aLpGMFW-OIJkXBE5scmGyTvbHBZ0ac7iMm5vxknHU53hXbkD7Z4s1SKX9PchFYL6oy8LLCQpuqWGW3xJUOF9-ffiCqxcLNWL33vBG0D05iSjyBuT18G59gn9Q1c18anKcJEQIFuDnVecFfSmXXv1o9Bkv6v1Xc5nJ6ROSlQG3NVjAAANx4Nb8Kj2zObuKKFsM0jAYSJAdkLZqwIx2JCdR3MB_12DfpCxMwtI-8uwFiKQnOHQ2S52x_igs6x7NEE-9keioTqk9AHQWad9lXUW4FKoILK_Lvi6pEsHtprA5Co5u2MhKWsn5pJekmqI5xV4qdQ6jpp-uLVBmmVoObwtwJljdDKLcth2pvwh7AlcRdffBehjWN3Rs9sFeJPzBCeXy3RbVdLzVyRriYdxOd3UiIggaAc4gMo91tfUT6d80k44orzaYzzJS77xYOod8PY5qt_jYLFm3Ikda2i2Jp3AURqhUcXdFSNqbs5zCV3eKGOpRCKLFI6w3DDUo3Sc_DhdiSH7y4cekWZcKk_i24zdD9GgSZcJoP3HpCW3tF_3gi42GBGl_NX0TzvaQQ16mvQPnjJjnIE-tl1GVvhp6xf5ZkX2LQSb6KYkYNlUrifmi8Yjwsjd_P9iZKoU1I1dB3x6UWPwYk1WFYsPzRhMoMBICWQlhZFgFUaOlisZGIQ-L_u3cTCY7Wf6tWciUkcfZbAr7WzUC8QYO9ljbBf8KEil_mpFXM-2hjnx5T2IqjK_zIRw1h3IxTu1_Xo2-bSc6aPbOEaUZM0icL8pej86cRn8gYPb2K_GPlwQeAcQS8aRoHbW34ZhPdKSs8c4KygUlmCHP08nY2AI3vCGjcHcrmCnXmcNUsJSSbK1mfuZ46F8T1QvSdoZMkXQ6nZ2dzMhSIWlI9aSuCjcFEEGpGCQtJydcyb_mW4xhySJdqr_Tu2wvGTPZUmnuruElMJta1CDPvsWr7nATp5yzuqoy69XHkosz8V0VNbSTb3-AibBHQ5Y8Iiw8-h_NuazdwLcZnHWeoNQKOo4D_niu9HLwMWHYyybWwJrlxGZFb5MSv8L42ioY4sO6rAMJg9KlcNXxgcm0tQY7obORR7JW5rAsikOvnWMXoRdzpwEN5w5jL0CvKNzmaQ1Bjm48T7WVEE8YcCBULkyp-z0KlR-cGVO4FrEb-0URN_B3pG313ZQ7WiUSQHZxA3PCrb1H3JoViKDskN4Fg3hveGPO5hztywDV2s16tjmG8GwcVQTFIfEYB36yQuDHszTITPquKqmr7__8VqZzy60jmxG0673xdI4-YYP2RBDCDuweqFWvy8coOx8vcfahzjvVuU6s3r77g4nbMJLLxs39sjyKtKixkJQbEoeWbJKDZzd_Rdmqq6LFfM9BJMzvWI1TaBAh76zgvEdEtcIMWtU8af1rDQCnOi9mKBf3cYnA2WIzzjgkQAAXBghK8-aMGPxAci1CdE6zT0be1PzYuuuO3tQMVN1nhASydXcraEN78QWkRuhQS6uT7-2IO_972Bq8pwkQvrs1-QI_Ogkaf6unCv_aMQQPuZOtI54wq68cUX12eFA3Gl4vRWibNau7dhb_n0Pw6N96jiQ.Bct10Kov3BpDACdbbbk_zsjvjaLg4vaPPrJefOce4Co"

reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")

# signingString must be declared exactly as shown below in separate lines
signingString="(request-target): $httpMethod $reqPath
date: $reqDate
digest: $digest"

# signingString must be declared exactly as shown below in separate lines
signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_client_signing.key" | openssl base64 -A`

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X GET "${httpHost}$reqPath" \
-H "Accept: application/json" \
-H "Content-Type: application/json" \
-H "Digest: ${digest}" \
-H "Date: ${reqDate}" \
-H "Authorization: Bearer ${caccessToken}" \
-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
--cert "${certPath}example_client_tls.cer" \
--key "${certPath}example_client_tls.key"
