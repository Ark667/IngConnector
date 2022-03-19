#!bin/bash

###################################################################################
#                             REQUEST URL TO ING AUTHORIZATION APP                #
###################################################################################
# This script calls the endpoint "oauth2/authorization-server-url" to request     #
# an authorization code for requesting customer access token. In this script      #
# we pass "payment-account:balance:view" and "payment-accounts:transactions:view" #
# scope tokens to consume AIS API. You must request an application access token   #
# to run this script. Please update the variables "accessToken" and "certPath".   #
###################################################################################


keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
certPath="./certs/" # path of the downloaded certificates and keys
httpHost="https://api.sandbox.ing.com"

# httpMethod must be in lower code
httpMethod="get"
reqPath="/oauth2/authorization-server-url?scope=payment-accounts%3Abalances%3Aview%20payment-accounts%3Atransactions%3Aview&redirect_uri=https://www.example.com&country_code=NL"

# Digest value for an empty body
digest="SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="


# Generated value of the application access token. Please note that the access token expires in 15 minutes
accessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..DtAeaXrG7-93OZxg9Nfr1A.xbfOO2WqrV0fcgJA-9tyoC8y8_pJuaBPPEWiRitQHZCBjchVij9NTebNncCkhFRIPR5WADSAq_1Qe-oIXso2aAfWTdN__EblEf6ZcOsEjg-4mrp3ge_mLMSdEFoT2Dz0XUFetA0APvxVpcoPr0MWj5TGQAe4sYTD7CEFBjLeAFrOK2CPMiaFZgfvsgoDCTqAki-0mZL0KF9Y18DEBfTwPN8nHk0hBglIHxc5le7bhgT7mObPhOMbO3aVKjUxtudkP6r8lo9oOBaxgTfH03g7mSMv4edPVQTiS4ogWVOOFvdFXOfzZ3Z-w_52ocWfoiiSbrjntSJ4dF6j_cBaR6ZhMotqcvmuebHvKohdcUIDq9_WbfcijIok5DMWXih2SjjJbMxGb86ULOSnCVAANAvA0PJ6ilusvJnAEDqdiQJ7UMshyPr8g67DMPnyQqRJerJtkCmXySBMOoopVq-_tQBxztpU4YUuKXRgRxx1LjgjjigdZ5My_X3XzxCRKtoua9rwlsVAJxmdHjIyTrH2GMavp9xgBYe8gWrRn6KMvj1e3joTelrUr4pIbFBJiOUXcHhbmPKHi4mXhR8RLSeHG7W_bLN00o0t0ivizbKLuFEZ65x62Xl8BIOHgwBGNsuKjOEDz0EB7wSiQgYULtR-I7hJNdA9qCo9REtLHYPegvagPrZX4Ldw57rr0EXusP3uaU7BfX-WHvhqItyjkNWVs_sNSjQEfOOJzyPOwZ0jOBQ2EXQ3tXFi8CnIc5ownR8IZGSNdc_Jv9H4p49APUkd8wFgqlpgFn85_koOZUj8Fnl4N8-Bivm6QXP8L5zIS0hvGuEg89QXVw3PVOCEEfIwMD9ngbEibnhqg8k2cPh0-1Fd_13WiYdmH6ZSEDM5qABZFVp0rtgZl84WXpj-E46bmyaOE91PHv7n2utMcg91Lmdv1KCi-xwqXTot9w749MFYjW8YvpMGHKFpeZkPdHhxXFFTWe1oQN1jyBelOrAjD7kUtvY5aLHVz6iStpxl8ILjqCgQNhviDx3N67N-jx6G1Wd4hJAdv28eU1whmPgyp4ijyoR4EByTRwkndVq0XzTGEZNY32_DQHlq1wmM2K5Sg6iXBFF1qgAjwvi9Tz1xkZSSZi3sKTPjfARbHrov4YRchb3TiKGEMESvbu-jB5qjx4UgnEf9M_XNtIDKrbb7BZNJqHorpkcVH4S6A7DQRVjN8WXNiQ_yWytoSwM7uXKSWbFvaTGsaM-BZfe4Nxg1KZ4j4q_OPcp7WJBnLV15i61NDPL6ryGWYSOkg2c822j34_1IE_r3pD59w0W4WBlsZ-ROPqNtG7rUh9CEQmFRjyV26_8OpcafRBA3pj9hOO0kctIPq0CoJak9M39xicMCWFYWoax-zaFBevqCG3lQcP3Eg8QcjwFVzBIcXs6SvfJGwHUjwszvBQQg5_Q2ym59ZBE-Bw5A-2McpF3URFqt6Cig002U9QnfxA_Hz2XmOfDLWeM6GwC8a1-NbV9kqioGr8lTa-Rvp43qZGPgGgFDsd7MT3Xt7TwVKTcSaz5iGR5P1-cPgNVD7Vv5WCuWN0EFbu4gpxPC72AKN0h1eDJP2QsW5eJXLP768HwyFjEgpxn_oICeTsuWrUa0Dne0U_d1JviALj0tBB3Z1WFUfE-iMtHq203kkE9s4LUz-Y3YokToZpvQZI6Yf0-LUKiA7CQ_znYjn629ozcA06AGx5ORCVvuLtUMC1P5ooiTh8A5NlXIvem-dLWCzXuZ-XLKK9_HSguG7fQQQIMq4Jgc2ZFYT78awhZF_rO0rX1oz6TuLVPJO9L9rO528YlThY4NMehZlUUR0JG7Xuaww0wu3HLWPG2tazdiOl37vOpaEZ5cVMndRPfbmUY8LTy-5BpFYW9KzMiQagww6qcYTQ4n_vCH_X8s4g7owU926CT2P2HxjMc_4YIHBppBqdqysjh66ORXuiksWIS3IOJZWtIsaQA-NKzvE7AsvNDMD6BIjZGZkqVSnxfhi959b5TCh2jnNmZp_xzX0gblTYLgWgn0F11jkC-DS1TLInZGECfO1vjI9WhxNZqerRmCguQnwwiZRVO4Dspk7g-b9mljRQvP_0KmizVgyis9BGeP5LdanTN_vZUaTBmkbIuRsISOAByelz75f7i1PubXYkwlyC7aXFf4yMAAQvckdcw6FEx44sVYD2MXwLZxRwY2CWaeTBUisBgUoMyJQr7KP_W6lcWbTOyv0G2vXP37hsGn6ICposO2drdpvgeDrRRdP33KTFhJOD0emtKui6U1HxqE3LV6mn4Bw6SnmgCOQkXh3zocxrLfsPOSbjxsOPEY--qV0912vd126w_3B9sEjdWr9A5CuTAMaHtMWV3Ks4t-pvGiAFMJaBx9yuXC41PIs6RkNshm4IIpukHFVf1Hyh-EKonrdp1rGrUpsV_33avQASJXdY8CCqe85hOWEipuNg_Gzx7DDZymG6nqnh2Vyjqbzkvnjg-9Z-XdAkBx9ATtVn22siXSVSLaG9tn3cdfzF752AF_nCygJfN7W9WN5ImLatNYjqpiq3Jg-dWlHT1CzaT11ou04ZbzNGrE2Bqifc8BDlGomIRJVrgL6sQ.TlvdsGOo_MJvGLA_kEkhBvIYRxQ6dA-rKIoPWV7yh0k"

reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")

# signingString must be declared exactly as shown below in separate lines
signingString="(request-target): $httpMethod $reqPath
date: $reqDate
digest: $digest"

signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_client_signing.key" | openssl base64 -A`

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X GET "${httpHost}${reqPath}" \
-H "Accept: application/json" \
-H "Content-Type: application/json" \
-H "Digest: ${digest}" \
-H "Date: ${reqDate}" \
-H "Authorization: Bearer ${accessToken}" \
-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
-d "${payload}" \
--cert "${certPath}example_client_tls.cer" \
--key "${certPath}example_client_tls.key"
