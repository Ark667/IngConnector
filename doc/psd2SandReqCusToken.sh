#!bin/bash

###################################################################################
#                             REQUEST CUSTOMER ACCESS TOKEN                       #
###################################################################################
# This script requests customer access token based on authorization code using-   #
# the endpoint "oauth2/token". You must request an application access token to 	  #
# run this script. Please update the variables "accessToken", "certPath" and      #
# "authorization_code"      													  #
###################################################################################

keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
certPath="./certs/"  # path of the downloaded certificates and keys
authorization_code="ec2a8897-dced-4088-a00c-eeea1f147ab5" # generated value of authorization code from the previous step.

# Generated value of application access token. Please note that the access token expires in 15 minutes

accessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..4_zaznclWYV0dnjNK5jdKw.ecZWgnMYbiY8kGuavIKrKfbWLE0zkBqIvpnvKAgNs4A4Bg_QouJdHSggGAPFwr3uGq4TPDVTzuyZBqZGEk-WNDyvYfrWc0uI_KBb_l-JOtZTK0jk_Rj2h0gfzDlepk7qhLc9mSPJfvN5z9lNj8jgMwSIvJOOt5wsWD-yOJxI7NrTdlnQRmC1fWuCPdtm0g7dqE9HoCA4ebnEjipjHyD7FiiqoBA-y_o4XomAGdtU0XdTamXp8eeHkOG4suaMcW7oRinvdMCNmKklKX6I6hFTOXlT5yDlCmR30Qx1dOyJxTIe5B8IL9BxYdefnC8xz6kaLj54dgWA3THFT3D9I3zIk0q0sYuXOSF6iXMPfohX7w4L1hwukLYBgu8MaFjP-1GaERXryVwiGyzb3Meyakos8zCLKd7xmDqacop4z8cTeqo4Ge0Non_1XOaPwz3cyuW6alQ_aF76euiBpUVMF7i9NAUZNCgOGQKSR3zv0h7YNcfzHwE50OvNYuOxKTk1WSM9HMTZxamJxFYMpvf2TF280xUhD8TTDKCaWhlvIq7usgP-NDpVkpa3uVA3TDXSCr9MmX7gP61ETkD73Dly-p4Mk_QPVGqQC1wV_X2X8_4qRzpQEg1F0q3J7a_K8Z476cVE7z5rp6KosIc7S_vCBmZVp4277R00mdekkOTyXUwCJ2eFeACMLtdeG2yP-Sy9u2q1LGAocQ_KTwlvXqobFwQPZZrsBCXZnPQ1x2mlBl5NAMuWH36waBXVx0rzmHaYbg0hOq58ynEBQ_awkAHmtBRUKw-MV3gkKO2WFLypuiqhhSFqPwxzKyq7e6u0RZJovcfFekcbDBo_7YctIYdAqqwezHhsJtAiV3hacvcesW1KwVBwwMESFgQhq5slLFhUijEs-ZpsCAGlCBKGkFPTkkrvRd8yRfAtxDCZnkuMumjT7eeD3hsBXgu2OJzUp_v-FEiu9v4BDrLOW5ElQf79cj6ngxDRjhDiPd28RUhenspQF7lL6OVnwTpTgRf67_W9Io3g7QXjsoe5-BrsL1aiS4BrBBcb9Gez90_i0SsX5K_axTg-YIwq_2CH8MsAYFvqKplya6aKEwwKa9ca8F6i9g_rKcW75LVixhNJqJKGEGRKFbLYHyV7-3z3kR44_dnlAj2xVevjJd6wt0wzMPHcLA5sPn4MrWbcnsWxtGiFM3G0X7mrQfUyRHnzXHFfWujdLU5OJgw1nnT1e7fR0stkM0z_2ql6H2sURjZTS9DWpGDSxR0qC5cHo2pga39PoyVO_1Z_rK9U3qRgiua-WqkUK1jl7Uh5NvU9Wz0RkNNSnFZEpy5ioNYU1Rr0sbJjG_0JgYBN_g7Tk8p64e-rE9JYlT8EU0Jiae1KkJ97P2-XjECxkPfx9q589UVBH0wqKqnsVi7aaNA-iVOVXYzXpM3KgAATANc2Qle3PSduuVvWRjViUqfbLfYpFHM7qYppp6ZbwslpGuwbII9B4tah-RBEF9Mf_foVrvTsz0Hk-90NDKwCztAwaaMjl2ZE7KdyeTQ7uf3fShp1gKGaBPjVZZCwsN0tHCXKd5eWxrHAWRCKyTnkK9PmNaw7cp040WIX3CGatgAx6Zq9wxptN7q1ugil3lOv8l1tstfDFimTaz4_6ryL3LOWnsUU9EXuH-JivNOpKi7QczsPHNTZtEc5U-0QV864PhPFZanT9I-qnUWRzAmwo1vSzWY5rf86BJ94EsooqO4HL04f3YGZ-SW0x3fP9HEDqAkMkrSN1do6jkKcDwQA_RrRd-MhkL7OJ6hDhdvKsHrKvFIpJRz3zbCjN1AnToIJXoLZYn7JBdGuK0hAxcGbO4wd4uQggsGalGzGW6IXS6Ji0gD35Q_ZP0gs3sgpENYu33s2C_InIw0rPKpBoclmiQLTuG0t9Xsft4glK9zaSvZptbI4W9zNrO2HHFX74Vtxzb1uJ-GNgV11KmzpqDMlOfteWuK-eyXdc1RpT7Js1xY1LSQ8bHW9vuehFZkPDW8Lk9RUp7WZrOf3Ogdx_CcOgrQ9boE6jpajgx-WVNX9MdjoW3H8mx3ELC_P0a1bfXG5amkIwx-lalk-9FnFpTYOmbl8uvqS8kdqwEkB3coJGpXlVwGtTFsptqVR32oqhZjcVSg2KNi3WGX96PjnjjYaeAlPGHIXx73KNaxBXz63iEFyzYveXwno2X0nkdTMjTv5Io0fnWayNxuGKu_ZusfZpW3RSaVEL_VqteIT0_8RF21L-_BZKiKiPbBvCgH8qXqqRwUUzdkz22aZ7VnqL2LB4Dz_lw5v6zXYmeMCwONazaVc34PaJzsiinXtFjn6W6cCG4-Vaz_yMAOnTm16M0yelvWqzzWcF3BYv5Ze0owsR0zkVKxwDGTVBAKy-ZTm6sA55PL_ezAOyzfQKsetCKiOLV88omOGG_HS3HGGEKHA4DpKZv5pMkWLJWKqLtnXbrKGB1kDdBkGwO2fhk7HJgIvmmNYv2rlh0dx3EAmkMk8ElRdy_Z0Tjh1jGEZVJ1CTOPFbVrU-bII1iYS5SvUtPiky7kzBjMxDTLYyIEq60_WVyMO6jzq8Uk2RMca5XcBc1Iammg5py6_WjLQAPA3WpkKnW8.igZok-qfmv2zQ9UapLZHh5IVlTaXQpOpN8R5GGYPXok"

# AUTHORIZATION CODE MUST BE PROVIDED AS A VALUE TO THE "code" PARAMETER IN THE PAYLOAD.
payload="grant_type=authorization_code&code=$authorization_code"
payloadDigest=`echo -n "$payload" | openssl dgst -binary -sha256 | openssl base64`
digest=SHA-256=$payloadDigest

reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")

httpHost="https://api.sandbox.ing.com"

# httpMethod value must be in lower case
httpMethod="post"
reqPath="/oauth2/token"

# signingString must be declared exactly as shown below in separate lines
signingString="(request-target): $httpMethod $reqPath
date: $reqDate
digest: $digest"

signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_client_signing.key" | openssl base64 -A`

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X POST "${httpHost}${reqPath}" \
-H "Accept: application/json" \
-H "Content-Type: application/x-www-form-urlencoded" \
-H "Digest: ${digest}" \
-H "Date: ${reqDate}" \
-H "Authorization: Bearer ${accessToken}" \
-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
-d "${payload}" \
--cert "${certPath}example_client_tls.cer" \
--key "${certPath}example_client_tls.key"
