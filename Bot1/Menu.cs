namespace Bot1
{
    public class Menu
    {
        // PASSWORD SAFETY
        public string[] PasswordResponses =
        {
            @"Passwords are your first line of defence against cybercriminals.

A strong password should:
• Be at least 12 characters long
• Contain uppercase and lowercase letters
• Include numbers and symbols
• Be unique for every account

Avoid using names, birthdays, or common words.",

            @"Using the same password on multiple websites is dangerous.

If one account is compromised, attackers may gain access to your other accounts.

Consider using a password manager to generate and securely store unique passwords.",

            @"Multi-Factor Authentication (MFA) provides an additional layer of security.

Even if someone discovers your password, they will still need a second verification step to access your account.",

            @"Weak passwords are one of the leading causes of account breaches.

Examples of weak passwords include:
• Password123
• 123456
• Your name or birthdate

Always create passwords that are difficult to guess.",

            @"A good password should be memorable to you but difficult for others to predict.

Consider using a passphrase made up of several unrelated words combined with numbers and symbols."
        };

        // PHISHING
        public string[] PhishingResponses =
        {
            @"Phishing is a cyberattack where criminals impersonate trusted organisations to steal sensitive information.

Their goal is often to obtain:
• Passwords
• Banking information
• Personal details",

            @"Be cautious of messages that create urgency.

Examples include:
• 'Your account will be suspended!'
• 'Immediate action required!'

Scammers use pressure to encourage quick decisions.",

            @"Before clicking any link:

1. Check who sent it.
2. Hover over the link.
3. Verify the destination website.

If something seems suspicious, do not click.",

            @"Many phishing emails contain spelling mistakes, unusual greetings, or suspicious attachments.

Always inspect emails carefully before responding.",

            @"Legitimate companies rarely ask for passwords or sensitive information through email.

If in doubt, contact the organisation directly."
        };

        // SAFE BROWSING
        public string[] BrowsingResponses =
        {
            @"Safe browsing helps protect your personal information and devices.

Always look for:
🔒 HTTPS websites
🔒 Secure connections
🔒 Trusted sources",

            @"Avoid downloading files from unknown websites.

Many malware infections occur through unsafe downloads and fake software installers.",

            @"Keeping your browser updated is essential.

Security updates fix vulnerabilities that attackers may try to exploit.",

            @"Public Wi-Fi networks can expose your information.

When using public internet:
• Avoid online banking
• Log out after use
• Use a VPN when possible",

            @"Be careful when clicking advertisements and pop-ups.

Some malicious websites disguise harmful downloads as legitimate software."
        };

        // MALWARE
        public string[] MalwareResponses =
        {
            @"Malware is malicious software designed to damage systems or steal information.

Common examples include:
• Viruses
• Worms
• Trojans
• Ransomware",

            @"Ransomware encrypts your files and demands payment to restore access.

Regular backups are one of the best protections against ransomware attacks.",

            @"Installing software from untrusted sources increases the risk of malware infections.

Always download software from official websites.",

            @"Antivirus software helps detect and remove threats before they cause harm.

Keep your antivirus updated to ensure maximum protection.",

            @"Operating system updates often contain important security patches.

Ignoring updates may leave your device vulnerable to attacks."
        };

        // PRIVACY
        public string[] PrivacyResponses =
        {
            @"Online privacy is about controlling who can access your personal information.

Be careful when sharing:
• Home address
• Phone number
• Banking details
• Identity documents",

            @"Review app permissions regularly.

Some applications request access to:
• Camera
• Microphone
• Location
• Contacts

Only allow permissions that are necessary.",

            @"Strong privacy habits reduce the risk of identity theft.

Use privacy settings on social media and avoid oversharing personal information online.",

            @"Cybercriminals often collect information from social media profiles.

Avoid publicly sharing information that could be used to answer security questions.",

            @"Always log out of shared or public devices.

This helps prevent unauthorised access to your personal accounts."
        };
    }
}