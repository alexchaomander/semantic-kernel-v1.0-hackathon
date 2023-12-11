# SecureGenMask

SecureGenMask is a designed to enhance data privacy and security in the realm of LLMs. Developed for the [Semanic Kernel Hackathon], this innovative project focuses on preventing Personally Identifiable Information (PII) and sensitive data from being transmitted to Large Language Models.

## Key Features

- **Regular Expression Magic**: SecureGenMask employs Regular Expressions to intelligently identify and remove any PII from data before it reaches LLMS.
- **Seamless Integration with Semantic Kernel**: Leveraging the hook events of the Semantic Kernel, our solution seamlessly integrates into the workflow, ensuring robust protection without compromising performance.
- **Azure AI Services**: Leveraging optional Azure AI.


## Why SecureGenMask?

- **Enterprise-Grade Security**: Tailored for enterprise use cases, SecureGenMask addresses the critical need for heightened security when dealing with customer information, GDPR compliance, and privacy concerns related to AI.
- **Effortless Implementation**: With a user-friendly design, SecureGenMask can be effortlessly integrated into existing systems, minimizing the learning curve for developers.(TODO)
- **Performance Optimized**: Our solution prioritizes performance without sacrificing security, ensuring a smooth and efficient experience in handling sensitive data. (TODO)

## How It Works

1. **Data Ingress**: SecureGenMask intercepts data before it is transmitted to Language Models.
2. **Regular Expression Filtering**: PII and sensitive data are meticulously identified and removed using advanced Regular Expressions.
3. **Secure Transmission**: Cleaned data is then securely transmitted to Language Models, preventing any inadvertent exposure of confidential information.
4. **Optional Integration to Azure AI Service - This uses the PII detection capability of Azure AI.
5. * Can be use as a Plugin or Used as a standalone native function.


## Screenshots

<img width="1440" alt="Screenshot 2023-12-10 at 5 19 57 PM" src="https://github.com/adesokanayo/semantic-kernel-v1.0-hackathon/assets/5377446/a89243cc-b876-4587-bc3c-57eecaff019d">


## Use Cases

- **Customer Information Protection**: Safeguarding customer data is paramount. SecureGenMask ensures that no sensitive information is compromised during AI interactions.
- **GDPR Compliance**: With increasing regulatory scrutiny, SecureGenMask aids organizations in complying with GDPR requirements by preventing unauthorized exposure of personal data.


### Contact Info
Share your contact info so the SK team and the community can get in touch

**Name:**  Ayo Adesokan

**Email:**  adesokanayo@gmail.com


### Next steps 
- **Publish SK Function**: As Sk Development progresses, I believe there will be a repository where users can find Functions,just like Nuget, I would love this functionality further and publish it.
