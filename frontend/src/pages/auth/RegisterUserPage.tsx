"use client";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { registerUser } from "@/services/AuthService";
import { useNavigate } from "react-router-dom";
import { toast } from "@/components/ui/use-toast";
import { RegisterUser } from "@/interfaces/Auth/RegisterUser";
import BackgroundImgae from "@/assets/bg-v.jpg";

const formSchema = z.object({
  email: z.string().min(5).email(),
  firstName: z.string().min(5),
  lastName: z.string().min(5),
  location: z.string().min(5),
  phoneNumber: z.string().min(5),
  password: z.string().min(5),
});

const RegisterUserPage = () => {
  const navigate = useNavigate();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      firstName: "",
      lastName: "",
      location: "",
      phoneNumber: "",
      password: "",
    },
  });

  const onSubmit = async (values: z.infer<typeof formSchema>) => {
    var registerUserDto: RegisterUser = values;
    const register = await registerUser({ registerUser: registerUserDto });
    if (register) {
      navigate("/login");
      toast({ title: "User registered!" });
    } else {
      toast({ title: "Failed registration!" });
    }
  };

  return (
    <div className="container grid h-screen w-screen flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
      <div className="hidden h-full bg-muted lg:block">
        <img
          src={BackgroundImgae}
          alt="Your Image"
          className="h-[100vh] w-full"
        />
      </div>
      <div className="lg:p-8">
        <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
          <div className="flex flex-col space-y-2 text-center">
            <h1 className="text-2xl font-semibold tracking-tight">Register</h1>
          </div>

          <h1 className="text-3xl font-semibold mb-4 text-left">User</h1>
          <p className="text-sm font-light mb-4 text-left pb-8">
            Create a user account!
          </p>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
              <FormField
                control={form.control}
                name="firstName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">
                      First Name
                    </FormLabel>
                    <FormControl>
                      <Input {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="lastName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">
                      Last Name
                    </FormLabel>
                    <FormControl>
                      <Input {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="phoneNumber"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">
                      Phone Number
                    </FormLabel>
                    <FormControl>
                      <Input {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="location"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">Location</FormLabel>
                    <FormControl>
                      <Input {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">Email</FormLabel>
                    <FormControl>
                      <Input placeholder="saram@hotmail.com" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">Password</FormLabel>
                    <FormControl>
                      <Input placeholder="" type="password" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <Button className="w-full" type="submit">
                Register
              </Button>
              <Button
                className="w-full"
                variant={"outline"}
                type="submit"
                onClick={() => navigate("/sign-in")}
              >
                Login
              </Button>
            </form>
          </Form>
        </div>
      </div>
    </div>
  );
};

export default RegisterUserPage;
