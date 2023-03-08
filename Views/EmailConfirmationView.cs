namespace timely_backend.Views; 

public static class EmailConfirmationView {
    public static string Page(string name, string url, string token) {
        return $$"""
        <!DOCTYPE html>
        <html lang="ru">
        <head>
            <meta charset="UTF-8">
            <title>Email Confirmation</title>
            <style>
                html, body {
                    height: 100%;
                    text-align: center;
                }

                body {
                    margin: 0;
                    color: #131313;
                    font-family: Verdana, sans-serif;
                }

                h1 {
                    font-size: 60px;
                }

                button {
                    font-size: 18px;
                    margin: 0 auto;
                    padding: 10px 20px 10px 20px;
                    color: #2e2f33;
                    border-radius: 4px;
                    font-family: Verdana, sans-serif;
                    cursor: pointer;
                }

                .info-container {
                    font-size: 18px;
                }

                .info-container > h2 {
                    font-weight: normal;
                    font-size: 28px;
                }

                .secondary-text {
                    color: #5c5b5b;
                    font-size: 14px;
                }
                
            </style>
        </head>
        <body>
        <div class="main-container">
            <div class="content">
                <div class="title-container">
                    <h1>Timely</h1>
                </div>
                <div class="info-container">
                    <h2>Здравствуйте, {{name}}!</h2>
                    <p>Вы указали данную электронную почту в профиле Timely.<br>
                        Пожалуйста, подтвердите её, чтобы воспользоваться всеми функциями сайта.
                    </p>
                    <p class="secondary-text">Если вы не регистрировались в нашем сервисе, то пропустите данное сообщение</p>
                </div>
                <div class="button-bg">
                        <a href="https://{{url}}?token={{token}}">
                            <button>
                                Подтвердить
                            </button>
                        </a>
                    </div>
            </div>
        </div>
        </body>
        </html>
        """;
    } 
}