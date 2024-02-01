RabbitMQ, bir mesaj sıralama (message queuing) yazılımıdır ve açık kaynaklı bir AMQP (Advanced Message Queuing Protocol - Gelişmiş Mesaj Sıralama Protokolü) uygulamasıdır.
RabbitMQ, uygulamalar arasında iletişimde kullanılan bir ara katman sağlar ve dağıtık sistemler arasında mesaj alışverişi yapmayı kolaylaştırır.
İşte RabbitMQ'nun temel kavramları:

Exchange (Değişim): RabbitMQ üzerinde mesajların yönlendirilme ve dağıtım işlemlerini yöneten yapıdır. Exchange, mesajları belirli kriterlere göre kuyruklara yönlendirmek için kullanılır. Mesajlar, exchange üzerinden belirli kriterlere göre kuyruklara yönlendirilir.

Queue (Kuyruk): Mesajların geçici olarak depolandığı, FIFO (First-In-First-Out - İlk Giren İlk Çıkar) prensibine göre çalışan veri yapılarıdır. Kuyruklar, mesajları geçici olarak saklamak ve tüketicilere dağıtmak için kullanılır.

Publisher (Yayıncı): Mesajları RabbitMQ'ya gönderen uygulamadır. Yayıncılar, mesajları belirli bir exchange'e gönderirler ve exchange, mesajları kuyruklara yönlendirir.

Consumer (Tüketici): Mesajları RabbitMQ kuyruklarından alan ve işleyen uygulamadır. Tüketiciler, belirli bir kuyruktan mesajları alarak işlemlerini gerçekleştirirler.

RabbitMQ, mikroservis mimarileri, dağıtık sistemler ve asenkron iletişim gerektiren uygulamalarda kullanılır. Mesaj sıralama, uygulamalar arasında bağımsız çalışan ve birbirleriyle haberleşen sistemlerin etkili bir şekilde entegre edilmesine olanak tanır.
